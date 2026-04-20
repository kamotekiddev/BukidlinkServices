using InventoryAPI.Infrastructure;
using InventoryAPI.Models;
using MediatR;

namespace InventoryAPI.Features.InventoryItems.UpdateInventoryItemStock
{
    public class UpdateStockCommandHandler(AppDbContext dbContext) : IRequestHandler<UpdateStockCommand, InventoryItem>
    {
        public async Task<InventoryItem> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
        {
           var existingInventoryItem = await dbContext.Inventories.FindAsync(request.inventoryItemId, cancellationToken);

            if (existingInventoryItem is null) throw new Exception($"Cannot find inventory item with id: {request.inventoryItemId}");

            if(request.action == InventoryAction.Increase)
            {
                existingInventoryItem.IncreaseQuantity(request.count);
            }

            if(request.action == InventoryAction.Decrease)
            {
                existingInventoryItem.DecreaseQuantity(request.count);
            }

            await dbContext.SaveChangesAsync(cancellationToken);
            return existingInventoryItem;
        }
    }
}
