using BuildingBlocks.Contracts.Product;
using BuildingBlocks.Exceptions;
using OrderAPI.Infrastructure.HttpClients.ProductServiceClient.Models;

namespace OrderAPI.Infrastructure.HttpClients.ProductServiceClient;

public class ProductServiceClient(
    HttpClient client,
    ILogger<ProductServiceClient> logger
) : IProductServiceClient
{
    public async Task<IReadOnlyList<ProductVariantDto>> GetProductVariantsByIds(ICollection<Guid> variantIds,
        CancellationToken ct)
    {
        var request = new GetProductVariantByIdsRequest(variantIds);

        var response = await client.PostAsJsonAsync("/variants/ids", request, ct);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<IEnumerable<ProductVariantDto>>(ct);

        if (result is null)
        {
            logger.LogError("Product service returned empty response. VariantIds:{VariantIds}",
                string.Join(",", variantIds));
            throw new BadRequestException("Product service returned empty response.");
        }

        return result.ToList();
    }
}