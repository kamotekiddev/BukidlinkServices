using Carter;
using MediatR;

namespace InventoryAPI.Features.Inventories.ReserveStocks;

public class ReserveStocksEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/inventories/reserve-stocks",
            async (ReserveStocksCommand request, IMediator sender) =>
            {
                await sender.Send(request);
                return Results.Ok(new { Success = true, Message = "Reservations successful." });
            });
    }
}