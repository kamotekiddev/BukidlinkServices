using InventoryAPI.Models;
using InventoryAPI.Models.Enums;
using MediatR;

namespace InventoryAPI.Features.Inventories.UpdateStock
{
    public record UpdateStockCommand(Guid InventoryItemId, int Count, InventoryAction Action) : IRequest<Inventory>;
}