using BuildingBlocks.Contracts.Product;

namespace OrderAPI.Infrastructure.HttpClients.ProductServiceClient;

public interface IProductServiceClient
{
    Task<IReadOnlyList<ProductVariantDto>> GetProductVariantsByIds(
        ICollection<Guid> variantIds,
        CancellationToken ct = default
    );
}