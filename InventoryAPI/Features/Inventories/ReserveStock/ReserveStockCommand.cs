using InventoryAPI.Models;
using MediatR;

namespace InventoryAPI.Features.Inventories.ReserveStock;

public record ReserveStockCommand(Guid InventoryItemId, int Quantity) : IRequest<Inventory>;