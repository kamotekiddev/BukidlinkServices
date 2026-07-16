using Auth.Domain;
using Auth.Infrastructure;
using Auth.Infrastructure.Auth;
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
        var user = await db.Users.FirstOrDefaultAsync(user => user.Email == request.Email, ct);

        if (user is null) throw new Exception("Invalid email or password");

        var passwordVerificationResult =
            passwordHasher.VerifyHashedPassword(user, user.HashedPassword, request.Password);

        if (passwordVerificationResult == PasswordVerificationResult.Failed)
            throw new Exception("Invalid email or password");


        var accessToken = tokenProvider.GenerateAccessToken(user);
        var refreshToken = tokenProvider.GenerateRefreshToken(user);


        return new LoginResult(accessToken, refreshToken.Token);
    }
}