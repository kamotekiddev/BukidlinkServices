using Carter;
using MediatR;
using ProductCatalogAPI.Features.Products.CreateProduct;

namespace ProductCatalogAPI.Features.Products.GetProducts;

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (IMediator mediator) =>
            {
                var products = await mediator.Send(new GetProductsQuery());
                return Results.Ok(
                    products.Select(p => new ProductDto(p.Id,
                        p.Name,
                        p.Description
                    ))
                );
            })
            .RequireAuthorization()
            .Produces<IEnumerable<ProductDto>>();
    }
}