using BuildingBlocks.Contracts;
using BuildingBlocks.Exceptions;
using InventoryAPI.Infrastructure;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Features.Inventories.ReleaseStock;

public class ReleaseStocksConsumer(
    AppDbContext db,
    ILogger<ReleaseStocksConsumer> logger
)
    : IConsumer<ReleaseStockEvent>
{
    public async Task Consume(ConsumeContext<ReleaseStockEvent> context)
    {
        var orderId = context.Message.OrderId;

        if (orderId == Guid.Empty)
            throw new BadRequestException("Invalid order id.");


        var reservations = await db.Reservations
            .Where(r => r.OrderId == orderId)
            .Include(i => i.Inventory)
            .ToListAsync();


        if (reservations.Count == 0)
        {
            logger.LogWarning("No stock reservation found for OrderId:{OrderId}", orderId);
            return;
        }

        foreach (var reservation in reservations)
        {
            var inventory = reservation.Inventory;
            inventory.ReleaseReservation(reservation);
        }

        db.Reservations.RemoveRange(reservations);
        await db.SaveChangesAsync();

        logger.LogInformation(
            "Released {Count} stock reservations for OrderId:{OrderId}",
            reservations.Count,
            orderId
        );
    }
}