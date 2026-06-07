using Carter;
using MediatR;

namespace OrderAPI.Features.Orders.GetOrders;

public class GetOrdersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders", async (ISender sender, CancellationToken ct) =>
        {
            var orders = await sender.Send(new GetOrdersQuery(), ct);
            return Results.Ok(orders);
        });
    }
}