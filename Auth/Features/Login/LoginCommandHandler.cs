using Auth.Domain;
using Auth.Infrastructure;
using Auth.Infrastructure.Auth;
using BuildingBlocks.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.Features.Login;

public class LoginCommandHandler(
    AppDbContext db,
    IPasswordHasher<User> passwordHasher,
    ITokenProvider tokenProvider
)
    : IRequestHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await db.Users.Include(user => user.Roles)
                       .SingleOrDefaultAsync(u => u.Email == request.Email, ct) ??
                   throw new BadRequestException("Invalid email or password");

        var passwordVerificationResult =
            passwordHasher.VerifyHashedPassword(user, user.HashedPassword, request.Password);

        if (passwordVerificationResult == PasswordVerificationResult.Failed)
            throw new BadRequestException("Invalid email or password");

        var accessToken = tokenProvider.GenerateAccessToken(user);
        var refreshToken = tokenProvider.GenerateRefreshToken(user);

        db.RefreshTokens.Add(refreshToken);
        await db.SaveChangesAsync(ct);

        return new LoginResult(accessToken, refreshToken.Token);
    }
}