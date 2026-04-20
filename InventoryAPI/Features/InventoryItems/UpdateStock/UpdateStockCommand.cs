using InventoryAPI.Models;
using MediatR;

namespace InventoryAPI.Features.InventoryItems.UpdateStock
{
    public record UpdateStockCommand(Guid InventoryItemId, int Count, InventoryAction Action) : IRequest<InventoryItem>;
}