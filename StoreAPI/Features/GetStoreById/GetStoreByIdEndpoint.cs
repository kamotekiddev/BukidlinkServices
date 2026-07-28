using Carter;
using MediatR;

namespace StoreAPI.Features.GetStoreById;

public sealed class GetStoreByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/stores/{storeId:guid}", async (Guid storeId, ISender sender) =>
        {
            var result = await sender.Send(new GetStoreByIdQuery(storeId));
            return Results.Ok(result);
        });
    }
}