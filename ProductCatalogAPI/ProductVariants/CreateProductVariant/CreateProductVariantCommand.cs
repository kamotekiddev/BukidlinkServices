using MediatR;
using ProductCatalogAPI.Domain;
using ProductCatalogAPI.Domain.ValueObjects;

namespace ProductCatalogAPI.ProductVariants.CreateProductVariant;

public abstract record CreateProductVariantCommand(Guid ProductId, string Name, string SkuValue, decimal Price)
    : IRequest<ProductVariant>;