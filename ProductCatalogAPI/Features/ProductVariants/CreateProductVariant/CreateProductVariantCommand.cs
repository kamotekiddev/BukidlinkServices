using MediatR;
using ProductCatalogAPI.Domain;

namespace ProductCatalogAPI.Features.ProductVariants.CreateProductVariant;

public record CreateProductVariantCommand(Guid ProductId, string Name, string SkuValue, decimal Price)
    : IRequest<ProductVariant>;