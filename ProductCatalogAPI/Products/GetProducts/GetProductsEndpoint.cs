namespace ProductCatalogAPI.Products.GetProducts;

public class GetProductsEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (GetProductsLogic getProductsLogic) =>
        {
            var products = await getProductsLogic.ExecuteAsync();
            return Results.Ok(products);
        });
    }
}