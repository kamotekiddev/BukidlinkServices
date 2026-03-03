using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Common.Exceptions;
using ProductCatalogAPI.Domain;
using ProductCatalogAPI.Infrastructure;

namespace ProductCatalogAPI.Products.CreateProduct;

public class CreateProductHandler(AppDbContext appDbContext) : IRequestHandler<CreateProductCommand, Product>
{
    public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var productExist =
            await appDbContext.Products.FirstOrDefaultAsync(p => p.Name == request.Name,
                cancellationToken: cancellationToken);

        if (productExist != null) throw new ProductAlreadyExistsException(request.Name);

        var product = new Product(request.Name, request.Description);
        appDbContext.Products.Add(product);
        await appDbContext.SaveChangesAsync(cancellationToken);

        return product;
    }
}