namespace InventoryAPI.Features.Inventories.ReserveStock;

public record ReserveStockRequest(int Quantity, Guid OrderId);