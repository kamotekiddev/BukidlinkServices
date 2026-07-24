using Auth.Infrastructure;
using Auth.Infrastructure.Auth;
using BuildingBlocks.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Auth.Features.Refresh;

public sealed class RefreshTokenCommandHandler(
    AppDbContext db,
    ITokenProvider tokenProvider
)
    : IRequestHandler<RefreshTokenCommand, RefreshTokenResult>
{
    public async Task<RefreshTokenResult> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        var existingRefreshToken =
            await db.RefreshTokens.Include(rt => rt.User)
                .SingleOrDefaultAsync(rt => rt.Token == request.RefreshToken, ct) ??
            throw new BadRequestException("Invalid refresh token.");

        if (existingRefreshToken.RevokedAt != null)
            throw new BadRequestException("Refresh token is revoked.");

        if (existingRefreshToken.ExpiresAt < DateTime.UtcNow)
            throw new BadRequestException("Refresh token is expired.");

        existingRefreshToken.Revoke();

        var user = existingRefreshToken.User;
        var newRefreshToken = tokenProvider.GenerateRefreshToken(user);
        var accessToken = tokenProvider.GenerateAccessToken(user);

        db.RefreshTokens.Add(newRefreshToken);
        await db.SaveChangesAsync(ct);

        return new RefreshTokenResult(accessToken, newRefreshToken.Token);
    }
}