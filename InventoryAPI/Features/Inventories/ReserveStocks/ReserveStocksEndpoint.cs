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
                var result = await sender.Send(request);
                return Results.Ok(result);
            });
    }
}