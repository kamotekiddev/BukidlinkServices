using Carter;
using MediatR;
using ProductCatalogAPI.Products.CreateProduct;

namespace ProductCatalogAPI.Products.GetProducts;

public class GetProductsModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (IMediator mediator) =>
            {
                var products = await mediator.Send(new GetProductsQuery());
                return Results.Ok(products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                }));
            })
            .Produces<IEnumerable<ProductDto>>();
    }
}