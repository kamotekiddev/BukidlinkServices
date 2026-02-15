using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Domain;
using ProductCatalogAPI.Infrastructure;

namespace ProductCatalogAPI.Products.GetProducts;

public class GetProductsLogic(AppDbContext dbContext)
{
    public async Task<IEnumerable<Product>> ExecuteAsync()
    {
        var products = await dbContext.Products.ToListAsync();
        return products;
    }
}