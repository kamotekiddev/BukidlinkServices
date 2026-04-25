using Carter;
using MediatR;

namespace InventoryAPI.Features.Inventories.ReserveStock;

public class ReserveStockEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/inventories/{inventoryId:guid}/stocks/reserve",
            async (Guid inventoryId, ReserveStockRequest request, IMediator sender) =>
            {
                var result =
                    await sender.Send(new ReserveStockCommand(inventoryId, request.Quantity, request.OrderId));
                return Results.Ok(result);
            });
    }
}