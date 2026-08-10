using InventoryAPI.Events;
using InventoryAPI.Infrastructure;
using InventoryAPI.Models;
using InventoryAPI.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Features.Inventories.UpdateStock
{
    public class UpdateStockCommandHandler(AppDbContext dbContext, IMediator sender)
        : IRequestHandler<UpdateStockCommand, Inventory>
    {
        public async Task<Inventory> Handle(UpdateStockCommand request, CancellationToken ct)
        {
            var inventory = await dbContext.Inventories
                .FirstOrDefaultAsync(i => i.Id == request.InventoryItemId, ct);

            if (inventory is null)
                throw new Exception($"Cannot find inventory item with id: {request.InventoryItemId}");

            if (request.Action == InventoryAction.Increase)
                inventory.IncreaseQuantity(request.Count);
            else if (request.Action == InventoryAction.Decrease)
                inventory.DecreaseQuantity(request.Count);
            else
                throw new Exception("Invalid action");

            await dbContext.SaveChangesAsync(ct);
            await sender.Publish(new StockUpdated(inventory.Id, request.Count, request.Action), ct);
            return inventory;
        }
    }
}