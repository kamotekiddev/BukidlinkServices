using MediatR;

namespace Auth.Features.Refresh;

public record RefreshTokenCommand(string RefreshToken) : IRequest<RefreshTokenResult>;