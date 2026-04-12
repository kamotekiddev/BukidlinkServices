namespace InventoryAPI.Infrastructure.Messaging;

public class ConsumerOptions
{
    public required string Exchange { get; init; }
    public required string RoutingKey { get; init; }
    public required string QueueName { get; init; }
}