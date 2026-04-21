using Carter;
using MediatR;

namespace InventoryAPI.Features.InventoryItems.ReserveStock;

public class ReserveStockEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/inventories/{inventoryItemId:guid}/stocks/reserve",
            async (Guid inventoryItemId, ReserveStockRequestDto request, IMediator sender) =>
            {
                var result = await sender.Send(new ReserveStockCommand(inventoryItemId, request.Quantity));
                return Results.Ok(result);
            });
    }
}