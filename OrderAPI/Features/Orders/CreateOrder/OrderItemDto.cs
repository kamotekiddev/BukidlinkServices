namespace OrderAPI.Features.Orders.CreateOrder;

public record OrderItemDto(
    Guid ProductVariantId,
    int Quantity
);