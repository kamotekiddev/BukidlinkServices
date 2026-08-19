using OrderAPI.Infrastructure.HttpClients.InventoryServiceClient.Models;

namespace OrderAPI.Infrastructure.HttpClients.InventoryServiceClient;

public interface IInventoryServiceClient
{
    Task<ReserveStocksResponse> ReserveStocksForVariants(
        Guid orderId,
        ICollection<ReservationItem> items,
        CancellationToken cancellation = default
    );
}