using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OrderAPI.Infrastructure.Messaging;

public class RabbitMqConsumer(RabbitMqConnectionFactory factory)
{
    public async Task ConsumeAsync<T>(
        ConsumerOptions options,
        Func<T, Task<bool>> handler,
        CancellationToken cancellationToken = default)
    {
        var connection = await factory.GetConnectionAsync();
        var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await channel.ExchangeDeclareAsync(
            exchange: options.Exchange,
            type: ExchangeType.Direct,
            durable: true,
            cancellationToken: cancellationToken);

        await channel.QueueDeclareAsync(
            queue: options.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken);

        await channel.QueueBindAsync(
            queue: options.QueueName,
            exchange: options.Exchange,
            routingKey: options.RoutingKey,
            cancellationToken: cancellationToken);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (_, args) =>
        {
            var json = Encoding.UTF8.GetString(args.Body.ToArray());
            var message = JsonSerializer.Deserialize<T>(json);

            if (message is null)
            {
                await channel.BasicAckAsync(args.DeliveryTag, false, cancellationToken);
                return;
            }

            try
            {
                var success = await handler(message);
                if (success)
                {
                    await channel.BasicAckAsync(args.DeliveryTag, false, cancellationToken);
                    return;
                }

                await channel.BasicNackAsync(args.DeliveryTag, false, requeue: true,
                    cancellationToken: cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await channel.BasicNackAsync(
                    args.DeliveryTag,
                    false,
                    requeue: false,
                    cancellationToken);
            }
        };

        await channel.BasicConsumeAsync(
            queue: options.QueueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: cancellationToken);

        // keep service alive
        await Task.Delay(Timeout.Infinite, cancellationToken);
    }
}