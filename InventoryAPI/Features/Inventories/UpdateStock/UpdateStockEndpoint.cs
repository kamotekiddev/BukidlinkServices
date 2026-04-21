using Carter;
using MediatR;

namespace InventoryAPI.Features.Inventories.UpdateStock

{
    public class UpdateStockEndpoint : ICarterModule
    {
        void ICarterModule.AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/inventories/{inventoryItemId:guid}/stocks",
                async (Guid inventoryItemId, UpdateStockRequestDto request, IMediator sender) =>
                {
                    var updatedInventoryItem =
                        await sender.Send(new UpdateStockCommand(inventoryItemId, request.Count, request.Action));
                    return Results.Ok(updatedInventoryItem);
                });
        }
    }
}