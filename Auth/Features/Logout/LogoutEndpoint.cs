using Carter;
using MediatR;

namespace Auth.Features.Logout;

public class LogoutEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/logout", async (LogoutCommand request, ISender sender) =>
        {
            var result = await sender.Send(request);
            return Results.Ok(result);
        });
    }
}