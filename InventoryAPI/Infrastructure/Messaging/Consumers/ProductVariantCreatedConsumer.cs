using InventoryAPI.Infrastructure.Messaging.Events;

namespace InventoryAPI.Infrastructure.Messaging.Consumers;

public class ProductVariantCreatedConsumer(
    RabbitMqConsumer consumer
)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await consumer.ConsumeAsync<ProductVariantCreatedEvent>(
            new ConsumerOptions
            {
                Exchange = "product.events",
                RoutingKey = "variant.created",
                QueueName = "product.variant.created"
            },
            async (message) => { Console.WriteLine(message.ToString()); },
            cancellationToken: cancellationToken);
    }
}