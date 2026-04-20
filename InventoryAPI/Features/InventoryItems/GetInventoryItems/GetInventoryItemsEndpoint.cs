using Carter;
using MediatR;

namespace InventoryAPI.Features.InventoryItems.GetInventoryItems
{
    public class GetInventoryItemsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/inventories", async (IMediator sender) =>
            {
                var inventoryItems = await sender.Send(new GetInventoryItemsQuery());
                return Results.Ok(inventoryItems);
            });
        }
    }
}
