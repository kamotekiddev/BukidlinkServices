using MediatR;

namespace OrderAPI.Features.Orders.CancelOrder;

public record CancelOrderCommand(Guid OrderId) : IRequest;