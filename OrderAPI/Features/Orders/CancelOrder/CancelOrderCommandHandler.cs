using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Infrastructure;
using OrderAPI.Models;

namespace OrderAPI.Features.Orders.CancelOrder;

public class CancelOrderCommandHandler(AppDbContext db) : IRequestHandler<CancelOrderCommand, Order>
{
    public async Task<Order> Handle(CancelOrderCommand request, CancellationToken ct)
    {
        var order = await db.Orders.FirstOrDefaultAsync(order => order.Id == request.OrderId, ct);
        if (order is null) throw new Exception($"Order with {request.OrderId} does not exist");

        order.Cancel();
        await db.SaveChangesAsync(ct);

        return order;
    }
}