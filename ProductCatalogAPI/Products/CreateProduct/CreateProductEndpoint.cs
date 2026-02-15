using FluentValidation;

namespace ProductCatalogAPI.Products.CreateProduct;

public class CreateProductEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/products",
            async (CreateProductRequest request, CreateProductLogic createProductLogic,
                IValidator<CreateProductRequest> validator) =>
            {
                var validationResult = await validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                    return Results.BadRequest(
                        validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

                var product = await createProductLogic.ExecuteAsync(request);
                return Results.Ok(product);
            });
    }
}