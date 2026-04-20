using InventoryAPI.Models;

namespace InventoryAPI.Features.InventoryItems.UpdateStock;

public record UpdateStockRequestDto(int Count, InventoryAction Action);