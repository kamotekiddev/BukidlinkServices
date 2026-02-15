namespace ProductCatalogAPI.Products.CreateProduct;

public class CreateProductEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/products",
            async (CreateProductRequest request, CreateProductLogic createProductLogic) =>
            {
                var product = await createProductLogic.ExecuteAsync(request);
                return Results.Ok(product);
            });
    }
}