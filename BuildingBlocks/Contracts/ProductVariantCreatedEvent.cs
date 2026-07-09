namespace BuildingBlocks.Contracts;

public record ProductVariantCreatedEvent(
    Guid VariantId,
    Guid ProductId,
    string Name,
    string Sku,
    decimal Price
);