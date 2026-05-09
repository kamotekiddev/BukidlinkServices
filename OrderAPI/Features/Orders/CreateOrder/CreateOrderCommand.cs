using MediatR;
using OrderAPI.Models;

namespace OrderAPI.Features.Orders.CreateOrder;

public record CreateOrderCommand(List<OrderItemDto> OrderItems) : IRequest<Order>;