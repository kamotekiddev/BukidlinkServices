using InventoryAPI.Features.InventoryItems.CreateInventoryItem;
using InventoryAPI.Infrastructure.Messaging.Events;
using MediatR;

namespace InventoryAPI.Infrastructure.Messaging.Consumers;

public class ProductVariantCreatedConsumer(
    RabbitMqConsumer consumer,
    IMediator mediator
) : BackgroundService
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
            async (message) =>
            {
                await mediator.Send(new CreateInventoryItemCommand(message.VariantId, 0, 0), cancellationToken);
            },
            cancellationToken: cancellationToken);
    }
}