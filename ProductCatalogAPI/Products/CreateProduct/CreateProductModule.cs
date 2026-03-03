using Carter;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ProductCatalogAPI.Products.CreateProduct;

public class CreateProductModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products",
                async (CreateProductCommand command, IValidator<CreateProductCommand> validator, IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    await validator.ValidateAndThrowAsync(command, cancellationToken);

                    var product = await mediator.Send(command, cancellationToken);
                    return Results.Created("products", new ProductDto()
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description
                    });
                })
            .WithName("Create Product")
            .Produces<ProductDto>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}