using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace ProductCatalogAPI.Infrastructure.Messaging;

public class RabbitMqPublisher(RabbitMqConnectionFactory factory)
{
    public async Task PublishAsync<T>(
        string exchange,
        string routingKey,
        T message)
    {
        var connection = await factory.GetConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(
            exchange: exchange,
            type: ExchangeType.Direct,
            durable: true
        );

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);


        await channel.BasicPublishAsync(
            exchange: exchange,
            routingKey: routingKey,
            body: body
        );
    }
}