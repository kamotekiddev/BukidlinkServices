using System.Security.Claims;
using Auth.Infrastructure;
using BuildingBlocks.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Auth.Features.Logout;

public class LogoutCommandHandler(AppDbContext db, IHttpContextAccessor context)
    : IRequestHandler<LogoutCommand, LogoutResult>
{
    public async Task<LogoutResult> Handle(LogoutCommand request, CancellationToken ct)
    {
        var idClaim = context.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(idClaim, out var userId))
            throw new UnAuthorizedException("User is not authenticated.");

        var refreshToken =
            await db.RefreshTokens.FirstOrDefaultAsync(
                rt => rt.Token == request.RefreshToken && rt.RevokedAt == null,
                ct
            ) ?? throw new UnAuthorizedException("Invalid refresh token.");

        if (userId != refreshToken.UserId || refreshToken.ExpiresAt <= DateTime.UtcNow)
            throw new UnAuthorizedException("Invalid refresh token.");

        refreshToken.Revoke();
        await db.SaveChangesAsync(ct);

        return new LogoutResult("The user has successfully logout");
    }
}