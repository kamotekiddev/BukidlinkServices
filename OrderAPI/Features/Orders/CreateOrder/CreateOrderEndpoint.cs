using Carter;
using MediatR;

namespace OrderAPI.Features.Orders.CreateOrder;

public class CreateOrderEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/orders/create",
            async (CreateOrderCommand request, IMediator sender, CancellationToken ct) =>
            {
                var order = await sender.Send(request, ct);
                return Results.Created($"/orders/{order.Id}", order);
            });
    }
}