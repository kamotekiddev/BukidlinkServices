using InventoryAPI.Models;
using MediatR;

namespace InventoryAPI.Features.InventoryItems.UpdateInventoryItemStock
{

    public record UpdateStockCommand(Guid inventoryItemId,int count, InventoryAction action) : IRequest<InventoryItem>;
}
