using BuildingBlocks.Constants;
using Carter;
using MediatR;
using ProductCatalogAPI.Features.Products.CreateProduct;

namespace ProductCatalogAPI.Features.Products.DeleteProduct;

public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{productId:guid}", async (Guid productId, IMediator mediator) =>
            {
                var deletedProduct = await mediator.Send(new DeleteProductCommand(productId));
                return Results.Ok(
                    new ProductDto(
                        deletedProduct.Id,
                        deletedProduct.Name,
                        deletedProduct.Description)
                );
            })
            .RequireAuthorization(Policy.Farmer)
            .Produces<ProductDto>();
    }
}