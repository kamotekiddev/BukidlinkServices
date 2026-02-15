using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Domain;
using ProductCatalogAPI.Infrastructure;

namespace ProductCatalogAPI.Products.CreateProduct;

public class CreateProductLogic(AppDbContext dbContext)
{
    public async Task<Product> ExecuteAsync(CreateProductRequest request)
    {
        var productExist = await dbContext.Products.FirstOrDefaultAsync(p => p.Name == request.Name);
        if (productExist != null) throw new Exception("Product already exists");

        var product = new Product(request.Name, request.Description);
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();
        return product;
    }
}