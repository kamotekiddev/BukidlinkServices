using BuildingBlocks.Exceptions;
using OrderAPI.Infrastructure.HttpClients.InventoryServiceClient.Models;

namespace OrderAPI.Infrastructure.HttpClients.InventoryServiceClient;

public sealed class InventoryServiceClient(
    HttpClient client,
    ILogger<InventoryServiceClient> logger
)
    : IInventoryServiceClient
{
    public async Task<ReserveStocksResponse> ReserveStocksForVariants(
        ReserveStocksRequest request,
        CancellationToken cancellation = default
    )
    {
        var response = await client.PostAsJsonAsync("/inventories/reserve-stocks", request, cancellation);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ReserveStocksResponse>(cancellation);

        if (result is null)
        {
            logger.LogError("Inventory service returned a empty or malformed response. Request:{@Request}", request);
            throw new BadRequestException("Inventory service returned a malformed response.");
        }

        return result;
    }
}