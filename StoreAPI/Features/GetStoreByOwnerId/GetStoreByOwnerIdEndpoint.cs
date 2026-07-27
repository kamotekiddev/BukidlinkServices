using Carter;
using MediatR;

namespace StoreAPI.Features.GetStoreByOwnerId;

public sealed class GetStoreByOwnerIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/stores/{storeId:guid}", async (Guid storeId, ISender sender) =>
        {
            var result = await sender.Send(new GetStoreByOwnerIdQuery(storeId));
            return Results.Ok(result);
        });
    }
}