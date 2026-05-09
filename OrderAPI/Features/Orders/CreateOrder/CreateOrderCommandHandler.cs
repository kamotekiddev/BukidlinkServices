using MediatR;
using OrderAPI.Infrastructure;
using OrderAPI.Models;

namespace OrderAPI.Features.Orders.CreateOrder;

public class CreateOrderCommandHandler(AppDbContext db) : IRequestHandler<CreateOrderCommand, Order>
{
    public async Task<Order> Handle(CreateOrderCommand request, CancellationToken ct)
    {
        var orderItems = request.OrderItems.Select(orderItem =>
                new OrderItem
                {
                    ProductVariantId = orderItem.ProductVariantId,
                    Quantity = orderItem.Quantity,
                    SellPrice = orderItem.SellPrice
                })
            .ToList();

        // TODO: user id will come from user session
        var order = Order.Create(Guid.NewGuid(), orderItems);

        db.Orders.Add(order);
        await db.SaveChangesAsync(ct);

        return order;
    }
}