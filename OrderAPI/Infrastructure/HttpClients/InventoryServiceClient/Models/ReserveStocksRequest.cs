namespace OrderAPI.Infrastructure.HttpClients.InventoryServiceClient.Models;

public record VariantRequest(Guid ProductVariantId, int Quantity);

public record ReserveStocksRequest(Guid OrderId, ICollection<VariantRequest> VariantRequests);