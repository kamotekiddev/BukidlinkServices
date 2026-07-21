using Carter;
using MediatR;

namespace Auth.Features.GetCurrentUser;

public class GetCurrentUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/user", async (ISender sender) =>
            {
                var currentUser = await sender.Send(new GetCurrentUserQuery());
                return Results.Ok(currentUser);
            })
            .RequireAuthorization();
    }
}