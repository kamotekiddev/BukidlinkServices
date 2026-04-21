using Carter;
using MediatR;

namespace InventoryAPI.Features.Inventories.GetInventories
{
    public class GetInventoriesEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/inventories", async (IMediator sender) =>
            {
                var inventoryItems = await sender.Send(new GetInventoriesQuery());
                return Results.Ok(inventoryItems);
            });
        }
    }
}
