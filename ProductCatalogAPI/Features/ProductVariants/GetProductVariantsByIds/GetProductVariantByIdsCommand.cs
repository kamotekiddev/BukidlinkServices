using BuildingBlocks.Contracts.Product;
using MediatR;

namespace ProductCatalogAPI.Features.ProductVariants.GetProductVariantsByIds;

public record GetProductVariantByIdsCommand(IEnumerable<Guid> VariantIds)
    : IRequest<IEnumerable<ProductVariantDto>>;