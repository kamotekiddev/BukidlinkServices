namespace BuildingBlocks.Contracts.Order;

public record OrderDto(
    Guid Id,
    Guid StoreId,
    string Status,
    string PaymentMethod,
    string PaymentStatus,
    IReadOnlyList<OrderItemDto> OrderItems,
    decimal TotalPrice);