using MediatR;
using OrderAPI.Models;

namespace OrderAPI.Features.Orders.CancelOrder;

public record CancelOrderCommand(Guid OrderId) : IRequest<Order>;