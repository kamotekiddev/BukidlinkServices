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

            switch(request.action)
            {
                case InventoryAction.Increase:
                    existingInventoryItem.IncreaseQuantity(request.count);
                    break;

                case InventoryAction.Decrease:
                    existingInventoryItem.DecreaseQuantity(request.count);
                    break;

                default:
                    throw new Exception("Invalid action");
            }
   
            await dbContext.SaveChangesAsync(cancellationToken);
            return existingInventoryItem;
        }
    }
}
