using BuildingBlocks.Auth;
using BuildingBlocks.Exceptions;
using MediatR;
using OrderAPI.Infrastructure;
using OrderAPI.Models;

namespace OrderAPI.Features.Orders.CreateOrder;

public class CreateOrderCommandHandler(
    AppDbContext db,
    ILogger<CreateOrderCommandHandler> logger,
    ICurrentUser currentUser
)
    : IRequestHandler<CreateOrderCommand, Order>
{
    public async Task<Order> Handle(CreateOrderCommand request, CancellationToken ct)
    {
        // check product variants, get info, active, price, storeid

        var userId = currentUser.UserId ?? throw new BadRequestException("Unauthenticated.");

        var orderItems = request.OrderItems.Select(orderItem =>
                new OrderItem
                {
                    ProductVariantId = orderItem.ProductVariantId,
                    Quantity = orderItem.Quantity,
                    SellPrice = orderItem.SellPrice
                })
            .ToList();

        logger.LogInformation(
            "Creating order. UserId:'{UserId}', StoreId: '{StoreId}', ItemCount:'{ItemCount}'",
            userId,
            request.StoreId,
            orderItems.Count
        );

        var order = Order.Create(
            userId,
            request.StoreId,
            orderItems
        );

        db.Orders.Add(order);
        await db.SaveChangesAsync(ct);

        logger.LogInformation(
            "Order created successfully. OrderId: '{OrderId}', UserId:'{UserId}', StoreId:'{StoreId}'",
            order.Id,
            userId,
            request.StoreId
        );

        return order;
    }
}