using MediatR;
using ProductCatalogAPI.Domain;

namespace ProductCatalogAPI.ProductVariants.CreateProductVariant;

public record CreateProductVariantCommand(Guid ProductId, string Name, string SkuValue, decimal Price)
    : IRequest<ProductVariant>;