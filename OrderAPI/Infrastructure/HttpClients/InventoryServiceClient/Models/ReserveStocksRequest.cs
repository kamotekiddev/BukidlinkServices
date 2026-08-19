namespace OrderAPI.Infrastructure.HttpClients.InventoryServiceClient.Models;

public record ReserveStocksRequest(Guid OrderId, ICollection<ReservationItem> Items);