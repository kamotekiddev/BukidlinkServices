using Auth.Domain;
using Auth.Infrastructure;
using Auth.Infrastructure.Auth;
using BuildingBlocks.Exceptions;
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
        await using var transaction = await db.Database.BeginTransactionAsync(ct);

        try
        {
            var role = await db.Roles.FirstOrDefaultAsync(role => role.Name == request.Role, ct) ??
                       throw new BadRequestException("The give role is invalid.");

            var existingUser = await db.Users
                .FirstOrDefaultAsync(user => user.Email == request.Email, ct);

            if (existingUser != null)
                throw new BadRequestException("User already exist.");

            var user = User.Create(
                request.Email,
                request.FirstName,
                request.LastName
            );

            user.AssignRole(role);

            var hashedPassword = passwordHasher.HashPassword(user, request.Password);

            user.SetPassword(hashedPassword);

            db.Users.Add(user);
            await db.SaveChangesAsync(ct);

            var accessToken = tokenProvider.GenerateAccessToken(user);
            var refreshToken = tokenProvider.GenerateRefreshToken(user);

            db.RefreshTokens.Add(refreshToken);
            await db.SaveChangesAsync(ct);

            await transaction.CommitAsync(ct);

            return new RegisterResult(accessToken, refreshToken.Token);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }
}