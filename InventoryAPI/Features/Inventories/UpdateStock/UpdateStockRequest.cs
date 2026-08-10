using InventoryAPI.Models.Enums;

namespace InventoryAPI.Features.Inventories.UpdateStock;

public record UpdateStockRequest(int Count, InventoryAction Action);