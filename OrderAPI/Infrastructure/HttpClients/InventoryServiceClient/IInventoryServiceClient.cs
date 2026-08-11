using OrderAPI.Infrastructure.HttpClients.InventoryServiceClient.Models;

namespace OrderAPI.Infrastructure.HttpClients.InventoryServiceClient;

public interface IInventoryServiceClient
{
    Task<ReserveStocksResponse> ReserveStocksForVariants(
        ReserveStocksRequest request,
        CancellationToken cancellation = default
    );
}