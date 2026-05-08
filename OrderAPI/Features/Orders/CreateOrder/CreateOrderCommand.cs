using MediatR;
using OrderAPI.Models;

namespace OrderAPI.Features.Orders.CreateOrder;

public record CreateOrderCommand(Guid productVariantId) : IRequest<Order>;