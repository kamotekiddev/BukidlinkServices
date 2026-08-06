using BuildingBlocks.Contracts.Store;
using BuildingBlocks.Exceptions;

namespace OrderAPI.Infrastructure.HttpClients.StoreServiceClient;

public sealed class StoreServiceClient(
    HttpClient client,
    ILogger<StoreServiceClient> logger
) : IStoreServiceClient
{
    public async Task<StoreDto> GetStoreByIdAsync(Guid storeId, CancellationToken ct)
    {
        using var response = await client.GetAsync($"/stores/{storeId}", ct);

        response.EnsureSuccessStatusCode();

        var store = await response.Content.ReadFromJsonAsync<StoreDto>(ct);

        if (store is null)
        {
            logger.LogError("Store service returned an empty response for StoreId '{StoreId}'.", storeId);
            throw new NotFoundException("Store not found.");
        }

        return store;
    }
}