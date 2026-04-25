using InventoryAPI.Models;

namespace InventoryAPI.Features.Inventories.UpdateStock;

public record UpdateStockRequest(int Count, InventoryAction Action);