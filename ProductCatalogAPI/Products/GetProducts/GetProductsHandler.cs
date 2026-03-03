using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Domain;
using ProductCatalogAPI.Infrastructure;

namespace ProductCatalogAPI.Products.GetProducts;

public class GetProductsHandler(AppDbContext dbContext) : IRequestHandler<GetProductsQuery, IEnumerable<Product>>
{
    public async Task<IEnumerable<Product>> Handle(GetProductsQuery request, CancellationToken ct)
    {
        var products = await dbContext.Products.ToListAsync(ct);
        return products;
    }
}