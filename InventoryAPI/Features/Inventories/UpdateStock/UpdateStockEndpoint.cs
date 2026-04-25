using Carter;
using MediatR;

namespace InventoryAPI.Features.Inventories.UpdateStock

{
    public class UpdateStockEndpoint : ICarterModule
    {
        void ICarterModule.AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/inventories/{inventoryId:guid}/stocks",
                async (Guid inventoryId, UpdateStockRequest request, IMediator sender) =>
                {
                    var updatedInventoryItem =
                        await sender.Send(new UpdateStockCommand(inventoryId, request.Count, request.Action));
                    return Results.Ok(updatedInventoryItem);
                });
        }
    }
}