using InventoryAPI.Infrastructure;
using InventoryAPI.Models;
using MediatR;

namespace InventoryAPI.Features.Inventories.UpdateStock
{
    public class UpdateStockCommandHandler(AppDbContext dbContext) : IRequestHandler<UpdateStockCommand, Inventory>
    {
        public async Task<Inventory> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
        {
            var existingInventoryItem =
                await dbContext.Inventories.FindAsync(request.InventoryItemId, cancellationToken);

            if (existingInventoryItem is null)
                throw new Exception($"Cannot find inventory item with id: {request.InventoryItemId}");

            switch (request.Action)
            {
                case InventoryAction.Increase:
                    existingInventoryItem.IncreaseQuantity(request.Count);
                    break;

                case InventoryAction.Decrease:
                    existingInventoryItem.DecreaseQuantity(request.Count);
                    break;

                default:
                    throw new Exception("Invalid action");
            }

            await dbContext.SaveChangesAsync(cancellationToken);
            return existingInventoryItem;
        }
    }
}