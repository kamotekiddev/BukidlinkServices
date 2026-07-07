namespace BuildingBlocks.Messaging.RabbitMq;

public sealed class RabbitMqOptions
{
    public string Host { get; init; }
    public string Username { get; init; }
    public string Password { get; init; }
    public int Port { get; init; }
}