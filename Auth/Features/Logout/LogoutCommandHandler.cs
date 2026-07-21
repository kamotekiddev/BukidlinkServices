using Auth.Infrastructure;
using BuildingBlocks.Auth;
using BuildingBlocks.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Auth.Features.Logout;

public class LogoutCommandHandler(
    AppDbContext db,
    ICurrentUser currentUser
)
    : IRequestHandler<LogoutCommand, LogoutResult>
{
    public async Task<LogoutResult> Handle(LogoutCommand request, CancellationToken ct)
    {
        var refreshToken =
            await db.RefreshTokens.FirstOrDefaultAsync(
                rt => rt.Token == request.RefreshToken &&
                      rt.UserId == currentUser.UserId &&
                      rt.RevokedAt == null,
                ct
            ) ?? throw new UnAuthorizedException("Invalid refresh token.");

        if (currentUser.UserId != refreshToken.UserId || refreshToken.ExpiresAt <= DateTime.UtcNow)
            throw new UnAuthorizedException("Invalid refresh token.");

        refreshToken.Revoke();
        await db.SaveChangesAsync(ct);

        return new LogoutResult("The user has successfully logout");
    }
}