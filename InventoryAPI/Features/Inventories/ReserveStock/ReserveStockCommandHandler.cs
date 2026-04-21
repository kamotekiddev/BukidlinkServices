using InventoryAPI.Infrastructure;
using InventoryAPI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Features.Inventories.ReserveStock;

public class ReserveStockCommandHandler(AppDbContext dbContext)
    : IRequestHandler<ReserveStockCommand, Inventory>
{
    public async Task<Inventory> Handle(ReserveStockCommand request, CancellationToken cancellationToken)
    {
        var inventoryItem = await dbContext.Inventories
            .Include(inventoryItem => inventoryItem.Reservations)
            .FirstOrDefaultAsync(inventoryItem => inventoryItem.Id == request.InventoryItemId, cancellationToken);

        if (inventoryItem is null)
            throw new Exception($"Cannot find inventory item with id: {request.InventoryItemId}");

        inventoryItem.Reserve(request.Quantity);
        await dbContext.SaveChangesAsync(cancellationToken);
        return inventoryItem;
    }
}