using Carter;
using MediatR;

namespace OrderAPI.Features.Orders.CancelOrder;

public class CancelOrderEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/orders/{orderId:guid}",
            async (IMediator sender, Guid orderId, CancellationToken ct) =>
            {
                var order = await sender.Send(new CancelOrderCommand(orderId), ct);
                return Results.Ok(order);
            });
    }
}