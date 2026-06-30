using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace OrderAPI.Infrastructure.Messaging;

public class RabbitMqPublisher(RabbitMqConnectionFactory factory) : IEventPublisher
{
    public async Task PublishAsync<T>(
        PublisherOptions options,
        T message, CancellationToken cancellationToken = default)
    {
        var connection = await factory.GetConnectionAsync();
        var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await channel.ExchangeDeclareAsync(
            options.Exchange,
            ExchangeType.Direct,
            true, cancellationToken: cancellationToken);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);


        await channel.BasicPublishAsync(
            options.Exchange,
            options.RoutingKey,
            body, cancellationToken);
    }
}