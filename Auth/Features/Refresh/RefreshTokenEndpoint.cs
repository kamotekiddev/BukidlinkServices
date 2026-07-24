using Carter;
using MediatR;

namespace Auth.Features.Refresh;

public class RefreshTokenEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/refresh", async (RefreshTokenCommand request, ISender sender) =>
        {
            var result = await sender.Send(request);
            return Results.Ok(result);
        });
    }
}