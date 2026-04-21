using InventoryAPI.Infrastructure;
using InventoryAPI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Features.InventoryItems.ReserveStock;

public class ReserveStockCommandHandler(AppDbContext dbContext)
    : IRequestHandler<ReserveStockCommand, InventoryItem>
{
    public async Task<InventoryItem> Handle(ReserveStockCommand request, CancellationToken cancellationToken)
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