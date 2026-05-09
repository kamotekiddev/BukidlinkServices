using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Infrastructure;
using OrderAPI.Models;

namespace OrderAPI.Features.Orders.GetOrders;

public class GetOrdersQueryHandler(AppDbContext db) : IRequestHandler<GetOrdersQuery, List<Order>>
{
    public async Task<List<Order>> Handle(GetOrdersQuery request, CancellationToken ct)
    {
        var orders = await db.Orders.Include(order => order.OrderItems).ToListAsync(ct);
        return orders;
    }
}