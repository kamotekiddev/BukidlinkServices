using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Common.Exceptions;
using ProductCatalogAPI.Domain;
using ProductCatalogAPI.Infrastructure;

namespace ProductCatalogAPI.Features.Products.CreateProduct;

public class CreateProductHandler(AppDbContext db) : IRequestHandler<CreateProductCommand, Product>
{
    public async Task<Product> Handle(CreateProductCommand request, CancellationToken ct)
    {
        var existingProduct =
            await db.Products.FirstOrDefaultAsync(
                product =>
                    product.Name == request.Name &&
                    product.StoreId == request.StoreId, ct
            );


        if (existingProduct is not null)
            throw new ProductAlreadyExistsException(request.Name);

        var product = Product.Create(
            request.Name,
            request.Description,
            request.StoreId
        );

        db.Products.Add(product);
        await db.SaveChangesAsync(ct);

        return product;
    }
}