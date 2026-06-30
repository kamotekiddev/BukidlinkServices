using BuildingBlocks.Contracts;
using MediatR;
using OrderAPI.Infrastructure;
using OrderAPI.Infrastructure.Messaging;
using OrderAPI.Models;

namespace OrderAPI.Features.Orders.CreateOrder;

public class CreateOrderCommandHandler(AppDbContext db, IEventPublisher publisher)
    : IRequestHandler<CreateOrderCommand, Order>
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

        await publisher.PublishAsync(new PublisherOptions
            {
                Exchange = "order.events",
                RoutingKey = "order.created"
            },
            new OrderPlacedEvent(order.Id, order.UserId, order.Status.ToString()),
            ct);

        return order;
    }
}