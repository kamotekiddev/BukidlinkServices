using Carter;
using MediatR;

namespace OrderAPI.Features.Orders.CancelOrder;

public class CancelOrderEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/orders/{orderId:guid}/cancel",
            async (IMediator sender, Guid orderId, CancellationToken ct) =>
            {
                await sender.Send(new CancelOrderCommand(orderId), ct);

                return Results.Ok(new
                {
                    Message = "Successfully cancelled.",
                    OrderId = orderId,
                    Status = StatusCodes.Status200OK
                });
            });
    }
}