using MediatR;

namespace Auth.Features.Logout;

public record LogoutCommand(string RefreshToken) : IRequest<LogoutResult>;