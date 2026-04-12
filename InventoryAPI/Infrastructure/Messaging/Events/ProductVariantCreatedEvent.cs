namespace InventoryAPI.Infrastructure.Messaging.Events;

public record ProductVariantCreatedEvent(
    Guid VariantId,
    Guid ProductId,
    string Name,
    string Sku,
    decimal Price
);