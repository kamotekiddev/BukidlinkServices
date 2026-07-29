using MediatR;
using OrderAPI.Models;

namespace OrderAPI.Features.Orders.CreateOrder;

public record CreateOrderCommand(
    Guid StoreId,
    List<OrderItemDto> OrderItems
) : IRequest<Order>;