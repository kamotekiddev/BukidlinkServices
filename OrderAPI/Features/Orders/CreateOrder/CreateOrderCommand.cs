using BuildingBlocks.Enums;
using MediatR;
using OrderAPI.Models;

namespace OrderAPI.Features.Orders.CreateOrder;

public record CreateOrderCommand(
    Guid StoreId,
    List<OrderItemDto> OrderItems,
    PaymentMethod PaymentMethod
) : IRequest<Order>;