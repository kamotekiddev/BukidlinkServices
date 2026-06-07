namespace OrderAPI.Infrastructure.Messaging;

public class ConsumerOptions
{
    public string Exchange { get; set; } = default!;
    public string QueueName { get; set; } = default!;
    public string RoutingKey { get; set; } = default!;

    public string? DeadLetterExchange { get; set; }
    public int MaxRetries { get; set; } = 5;
    public ushort PrefetchCount { get; set; } = 1;
}