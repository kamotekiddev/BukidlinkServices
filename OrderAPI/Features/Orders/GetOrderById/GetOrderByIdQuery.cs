using BuildingBlocks.Contracts.Order;
using MediatR;

namespace OrderAPI.Features.Orders.GetOrderById;

public record GetOrderByIdQuery(Guid OrderId) : IRequest<OrderDto>;