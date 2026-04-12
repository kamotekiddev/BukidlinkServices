using System.Text;
using System.Text.Json;
using ProductCatalogAPI.Interface;
using RabbitMQ.Client;

namespace ProductCatalogAPI.Infrastructure.Messaging;

public class RabbitMqPublisher(RabbitMqConnectionFactory factory) : IEventPublisher
{
    public async Task PublishAsync<T>(
        PublisherOptions options,
        T message, CancellationToken cancellationToken = default)
    {
        var connection = await factory.GetConnectionAsync();
        var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await channel.ExchangeDeclareAsync(
            exchange: options.Exchange,
            type: ExchangeType.Direct,
            durable: true, cancellationToken: cancellationToken);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);


        await channel.BasicPublishAsync(
            exchange: options.Exchange,
            routingKey: options.RoutingKey,
            body: body, cancellationToken: cancellationToken);
    }
}