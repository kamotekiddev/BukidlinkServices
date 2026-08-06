using BuildingBlocks.Contracts.Product;
using OrderAPI.Infrastructure.HttpClients.ProductServiceClient.Models;

namespace OrderAPI.Infrastructure.HttpClients.ProductServiceClient;

public class ProductServiceClient(HttpClient client) : IProductServiceClient
{
    public async Task<IReadOnlyList<ProductVariantDto>> GetProductVariantsByIds(IEnumerable<Guid> variantIds)
    {
        var request = new GetProductVariantByIdsRequest(variantIds);

        var response = await client.PostAsJsonAsync("/variants/ids", request);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<IEnumerable<ProductVariantDto>>();

        return result?.ToList() ?? [];
    }
}