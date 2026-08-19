using Carter;
using MediatR;

namespace ProductCatalogAPI.Features.ProductVariants.GetProductVariantsByIds;

public class GetProductVariantByIdsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/variants/ids",
            async (GetProductVariantByIdsCommand request, ISender sender) =>
            {
                var productVariants = await sender.Send(request);
                return Results.Ok(productVariants);
            });
    }
}