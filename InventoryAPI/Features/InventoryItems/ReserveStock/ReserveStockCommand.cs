using InventoryAPI.Models;
using MediatR;

namespace InventoryAPI.Features.InventoryItems.ReserveStock;

public record ReserveStockCommand(Guid InventoryItemId, int Quantity) : IRequest<InventoryItem>;