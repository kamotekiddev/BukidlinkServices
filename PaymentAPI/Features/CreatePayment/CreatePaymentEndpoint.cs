using Carter;
using MediatR;

namespace PaymentAPI.Features.CreatePayment;

public class CreatePaymentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/payments/create", async (CreatePaymentCommand request, ISender sender) =>
            {
                var result = await sender.Send(request);
                return Results.Ok(result);
            })
            .WithName("CreatePaymentTransaction");
    }
}