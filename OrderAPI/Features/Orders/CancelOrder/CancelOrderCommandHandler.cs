using BuildingBlocks.Contracts;
using BuildingBlocks.Exceptions;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Infrastructure;

namespace OrderAPI.Features.Orders.CancelOrder;

public class CancelOrderCommandHandler(
    AppDbContext db,
    IPublishEndpoint publisher,
    ILogger<CancelOrderCommandHandler> logger
)
    : IRequestHandler<CancelOrderCommand>
{
    public async Task Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await db.Orders.SingleOrDefaultAsync(order => order.Id == request.OrderId, cancellationToken);

        if (order is null)
        {
            logger.LogWarning("Order is invalid. OrderId:{OrderId} ", request.OrderId);
            throw new NotFoundException($"Order with OrderId:{request.OrderId} does not exist.");
        }

        order.Cancel();

        await db.SaveChangesAsync(cancellationToken);

        await publisher.Publish(new ReleaseStockEvent(request.OrderId), cancellationToken);

        // TODO: perform payment refund

        logger.LogInformation("Order with OrderId:{OrderId} is successfully cancelled.", request.OrderId);
    }
}