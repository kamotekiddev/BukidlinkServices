using BuildingBlocks.Contracts.Product;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Infrastructure;

namespace ProductCatalogAPI.Features.ProductVariants.GetProductVariantsByIds;

public sealed class
    GetProductVariantByIdsCommandHandler(AppDbContext db)
    : IRequestHandler<GetProductVariantByIdsCommand, IEnumerable<ProductVariantDto>>
{
    public async Task<IEnumerable<ProductVariantDto>> Handle(
        GetProductVariantByIdsCommand request,
        CancellationToken ct
    )
    {
        var variants = await db.ProductVariants
            .AsNoTracking()
            .Where(variant => request.VariantIds.Contains(variant.Id))
            .Select(variant => new ProductVariantDto(
                variant.Id,
                variant.Sku.Value,
                variant.Name,
                variant.ProductId,
                variant.Price.Value,
                variant.Price.Currency,
                variant.Product.StoreId
            ))
            .ToListAsync(ct);

        return variants;
    }
}