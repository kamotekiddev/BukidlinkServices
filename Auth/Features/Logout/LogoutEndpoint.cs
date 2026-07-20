using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Features.Logout;

public class LogoutEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/logout", async ([FromBody] LogoutCommand request, ISender sender) =>
            {
                var result = await sender.Send(request);
                return Results.Ok(result);
            })
            .RequireAuthorization();
    }
}