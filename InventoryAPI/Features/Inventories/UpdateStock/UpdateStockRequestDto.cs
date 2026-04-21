using InventoryAPI.Models;

namespace InventoryAPI.Features.Inventories.UpdateStock;

public record UpdateStockRequestDto(int Count, InventoryAction Action);