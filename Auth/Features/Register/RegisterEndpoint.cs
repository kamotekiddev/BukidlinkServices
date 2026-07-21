using Carter;
using FluentValidation;
using MediatR;

namespace Auth.Features.Register;

public class RegisterEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/register",
            async (RegisterCommand request, ISender sender, IValidator<RegisterCommand> validator) =>
            {
                await validator.ValidateAndThrowAsync(request);

                var user = await sender.Send(request);
                return Results.Ok(user);
            });
    }
}