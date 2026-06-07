using MediatR;
using OrderAPI.Models;

namespace OrderAPI.Features.Orders.GetOrderById;

public record GetOrderByIdQuery(Guid OrderId) : IRequest<Order>;