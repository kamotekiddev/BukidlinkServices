namespace OrderAPI.Infrastructure.HttpClients.InventoryServiceClient.Models;

public record ReservationItem(Guid ProductVariantId, int Quantity);