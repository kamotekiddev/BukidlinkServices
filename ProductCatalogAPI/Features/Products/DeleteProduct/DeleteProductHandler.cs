using MediatR;
using ProductCatalogAPI.Common.Exceptions;
using ProductCatalogAPI.Domain;
using ProductCatalogAPI.Infrastructure;

namespace ProductCatalogAPI.Features.Products.DeleteProduct;

public class DeleteProductHandler(AppDbContext dbContext) : IRequestHandler<DeleteProductCommand, Product>
{
    public async Task<Product> Handle(DeleteProductCommand request, CancellationToken cancellation)
    {
        var product = await dbContext.Products.FindAsync(request.Id, cancellation);
        if (product is null) throw new ProductNotFoundException(request.Id);

        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync(cancellation);

        return product;
    }
}