using InventoryAPI.Features.Inventories.CreateInventory;
using InventoryAPI.Infrastructure.Messaging.Events;
using MediatR;

namespace InventoryAPI.Infrastructure.Messaging.Consumers;

public class ProductVariantCreatedConsumer(
    RabbitMqConsumer consumer,
    IServiceScopeFactory scopeFactory
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
                using var scope = scopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                await mediator.Send(new CreateInventoryCommand(message.VariantId, 0),
                    cancellationToken);

                return true;
            },
            cancellationToken: cancellationToken);
    }
}