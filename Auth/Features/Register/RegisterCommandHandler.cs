using Auth.Domain;
using Auth.Infrastructure;
using Auth.Infrastructure.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.Features.Register;

public class RegisterCommandHandler(
    AppDbContext db,
    IPasswordHasher<User> passwordHasher,
    ITokenProvider tokenProvider
)
    : IRequestHandler<RegisterCommand, RegisterResult>
{
    public async Task<RegisterResult> Handle(RegisterCommand request, CancellationToken ct)
    {
        var existingUser = await db.Users
            .FirstOrDefaultAsync(user => user.Email == request.Email, ct);

        if (existingUser != null)
            throw new Exception("User already exist.");


        var user = User.Create(
            request.Email,
            request.FirstName,
            request.LastName
        );

        var hashedPassword = passwordHasher.HashPassword(user, request.Password);

        user.SetPassword(hashedPassword);

        db.Users.Add(user);
        await db.SaveChangesAsync(ct);

        var accessToken = tokenProvider.GenerateAccessToken(user);
        var refreshToken = tokenProvider.GenerateRefreshToken(user);

        return new RegisterResult(accessToken, refreshToken.Token);
    }
}