using Carter;
using MediatR;

namespace OrderAPI.Features.Orders.GetOrderById;

public class GetOrderByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/{orderId:guid}", async (Guid orderId, IMediator sender, CancellationToken ct) =>
        {
            var order = await sender.Send(new GetOrderByIdQuery(orderId), ct);
            return Results.Ok(order);
        });
    }
}