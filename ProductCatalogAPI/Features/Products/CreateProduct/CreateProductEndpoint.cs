using BuildingBlocks.Constants;
using Carter;
using FluentValidation;
using MediatR;

namespace ProductCatalogAPI.Features.Products.CreateProduct;

public sealed class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products",
                async (CreateProductCommand command, IValidator<CreateProductCommand> validator, IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    await validator.ValidateAndThrowAsync(command, cancellationToken);

                    var product = await mediator.Send(command, cancellationToken);
                    return Results.Created("products", new ProductDto(product.Id, product.Name, product.Description));
                })
            .RequireAuthorization(Policy.Farmer);
    }
}