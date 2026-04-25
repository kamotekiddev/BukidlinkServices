using InventoryAPI.Events;
using InventoryAPI.Infrastructure;
using InventoryAPI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Features.Inventories.ReserveStock;

public class ReserveStockCommandHandler(AppDbContext dbContext, IMediator sender)
    : IRequestHandler<ReserveStockCommand, Inventory>
{
    public async Task<Inventory> Handle(ReserveStockCommand request, CancellationToken cancellationToken)
    {
        var inventory = await dbContext.Inventories
            .Include(inventoryItem => inventoryItem.Reservations)
            .FirstOrDefaultAsync(inventoryItem => inventoryItem.Id == request.InventoryItemId, cancellationToken);

        if (inventory is null)
            throw new Exception($"Cannot find inventory item with id: {request.InventoryItemId}");

        inventory.Reserve(request.Quantity, request.OrderId);

        await dbContext.SaveChangesAsync(cancellationToken);

        await sender.Publish(new StockReserved(inventory.Id, request.OrderId, request.Quantity, DateTime.UtcNow),
            cancellationToken);

        return inventory;
    }
}