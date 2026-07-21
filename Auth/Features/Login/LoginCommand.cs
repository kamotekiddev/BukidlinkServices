using MediatR;

namespace Auth.Features.Login;

public record LoginCommand(string Email, string Password) : IRequest<LoginResult>;