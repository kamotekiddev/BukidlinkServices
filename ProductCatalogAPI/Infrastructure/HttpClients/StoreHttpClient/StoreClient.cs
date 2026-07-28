using System.Net;
using BuildingBlocks.Contracts.Store;

namespace ProductCatalogAPI.Infrastructure.HttpClients.StoreHttpClient;

public class StoreClient(HttpClient client, ILogger<StoreClient> logger)
{
    public async Task<StoreDto?> GetStoreById(Guid storeId, CancellationToken ct = default)
    {
        logger.LogDebug("Requesting store '{StoreId}' from Store Service", storeId);

        try
        {
            using var response = await client.GetAsync($"/stores/{storeId}", ct);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<StoreDto>(ct) ??
                   throw new InvalidOperationException("Store service returned invalid response.");
        }

        catch (HttpRequestException ex)
        {
            logger.LogError(
                ex,
                "Failed to retrieve store {StoreId} from Store Service.",
                storeId
            );

            throw;
        }
    }
}