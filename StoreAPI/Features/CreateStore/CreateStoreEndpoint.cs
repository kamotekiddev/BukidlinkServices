using Carter;
using MediatR;

namespace StoreAPI.Features.CreateStore;

public sealed class CreateStoreEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/stores/create", async (CreateStoreCommand request, ISender sender) =>
        {
            var result = await sender.Send(request);
            return Results.Created($"/stores/${result.StoreId}",
                new
                {
                    Message = "Successfully created a store.",
                    StoreId = result.StoreId
                });
        });
    }
}