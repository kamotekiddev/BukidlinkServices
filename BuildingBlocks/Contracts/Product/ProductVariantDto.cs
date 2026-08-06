namespace BuildingBlocks.Contracts.Product;

public record ProductVariantDto(
    Guid VariantId,
    string Sku,
    string Name,
    Guid ProductId,
    decimal Price,
    string Currency,
    Guid StoreId
);