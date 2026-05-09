using MediatR;
using OrderAPI.Models;

namespace OrderAPI.Features.Orders.GetOrders;

public record GetOrdersQuery() : IRequest<List<Order>>;