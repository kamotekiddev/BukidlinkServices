using MediatR;
using OrderAPI.Models;
using OrderAPI.Models.Enums;

namespace OrderAPI.Features.Orders.CreateOrder;

public record CreateOrderCommand(
    Guid StoreId,
    List<OrderItemDto> OrderItems,
    PaymentMethod PaymentMethod
) : IRequest<Order>;