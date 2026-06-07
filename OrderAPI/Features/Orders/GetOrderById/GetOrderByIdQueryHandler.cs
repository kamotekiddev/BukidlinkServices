using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Infrastructure;
using OrderAPI.Models;

namespace OrderAPI.Features.Orders.GetOrderById;

public class GetOrderByIdQueryHandler(AppDbContext db) : IRequestHandler<GetOrderByIdQuery, Order>
{
    public async Task<Order> Handle(GetOrderByIdQuery request, CancellationToken ct)
    {
        var order = await db.Orders
            .Include(order => order.OrderItems)
            .FirstOrDefaultAsync(order => order.Id == request.OrderId, ct);

        if (order is null) throw new Exception($"Order with id {request.OrderId} not found");
        return order;
    }
}