using Carter;
using MediatR;

namespace InventoryAPI.Features.InventoryItems.UpdateInventoryItemStock
{
    public class UpdateStockEndpoint : ICarterModule
    {
        void ICarterModule.AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPatch("/inventories/{inventoryItemId:guid}/stocks", async (Guid inventoryItemId, UpdateStockCommand request, IMediator sender) =>
            {
                var updatedInventoryItem = await sender.Send(new UpdateStockCommand(inventoryItemId, request.count, request.action));
                Results.Ok(updatedInventoryItem);
            });
        }
    }
}
