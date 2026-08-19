namespace BuildingBlocks.Contracts.Order;

public record OrderItemDto(
    Guid Id,
    Guid OrderId,
    Guid ProductVariantId,
    int Quantity,
    decimal SellPrice
);