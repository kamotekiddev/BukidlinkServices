using MediatR;

namespace Auth.Features.Register;

public record RegisterCommand(
    string Email,
    string FirstName,
    string LastName,
    string Password,
    string ConfirmPassword,
    string Role
)
    : IRequest<RegisterResult>;