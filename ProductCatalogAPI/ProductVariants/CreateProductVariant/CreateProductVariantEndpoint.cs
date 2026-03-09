using Carter;
using MediatR;

namespace ProductCatalogAPI.ProductVariants.CreateProductVariant;

public class CreateProductVariantEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/variants", async (CreateProductVariantCommand command, IMediator mediator) =>
        {
            var variant = await mediator.Send(command);
            return Results.Created("/products/variants", variant);
        });
    }
}