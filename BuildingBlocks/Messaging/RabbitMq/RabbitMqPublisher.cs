using System.Text.Json;
using RabbitMQ.Client;

namespace BuildingBlocks.Messaging.RabbitMq;

public class RabbitMqPublisher(IConnectionFactory connectionFactory) : IMessagePublisher
{
    public async Task PublishAsync<T>(T message) where T : IIntegrationEvent
    {
        using var connection = await connectionFactory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        var exchange = typeof(T).Name;

        await channel.ExchangeDeclareAsync(
            exchange,
            ExchangeType.Fanout,
            true);

        var body = JsonSerializer.SerializeToUtf8Bytes(message);

        await channel.BasicPublishAsync(
            exchange,
            "",
            body);
    }
}