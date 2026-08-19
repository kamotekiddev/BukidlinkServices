using BuildingBlocks.Contracts.Order;
using BuildingBlocks.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Infrastructure;

namespace OrderAPI.Features.Orders.GetOrderById;

public class GetOrderByIdQueryHandler(AppDbContext db) : IRequestHandler<GetOrderByIdQuery, OrderDto>
{
    public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken ct)
    {
        var order = await db.Orders
            .Include(order => order.OrderItems)
            .FirstOrDefaultAsync(order => order.Id == request.OrderId, ct);

        if (order is null) throw new NotFoundException($"Order with id {request.OrderId} not found");

        var orderItems =
            order.OrderItems.Select(item =>
                    new OrderItemDto
                    (
                        item.Id,
                        item.OrderId,
                        item.ProductVariantId,
                        item.Quantity,
                        item.SellPrice
                    )
                )
                .ToList();

        return new OrderDto
        (
            order.Id,
            order.StoreId,
            order.Status.ToString(),
            order.PaymentMethod.ToString(),
            order.PaymentStatus.ToString(),
            orderItems,
            order.TotalPrice
        );
    }
}