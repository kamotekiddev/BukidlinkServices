using Auth.Infrastructure;
using BuildingBlocks.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Auth.Features.Logout;

public class LogoutCommandHandler(AppDbContext db) : IRequestHandler<LogoutCommand, LogoutResult>
{
    public async Task<LogoutResult> Handle(LogoutCommand request, CancellationToken ct)
    {
        var refreshToken =
            await db.RefreshTokens.FirstOrDefaultAsync(
                rt => rt.Token == request.RefreshToken && rt.RevokedAt == null,
                ct
            ) ??
            throw new UnAuthorizedException("Invalid refresh token.");

        if (refreshToken.ExpiresAt <= DateTime.UtcNow)
            throw new BadRequestException("Invalid refresh token.");

        // TODO: need to validate if the token belongs to the current user before revocation
        refreshToken.Revoke();
        await db.SaveChangesAsync(ct);

        return new LogoutResult("The user has successfully logout");
    }
}