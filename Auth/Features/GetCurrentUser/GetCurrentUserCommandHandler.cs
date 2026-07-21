using Auth.Infrastructure;
using BuildingBlocks.Auth;
using BuildingBlocks.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Auth.Features.GetCurrentUser;

public class GetCurrentUserCommandHandler(AppDbContext db, ICurrentUser currentUser)
    : IRequestHandler<GetCurrentUserQuery, GetCurrentUserResult>
{
    public async Task<GetCurrentUserResult> Handle(GetCurrentUserQuery request, CancellationToken ct)
    {
        var userId = currentUser.UserId ?? throw new UnAuthorizedException("User is not authenticated.");

        var user = await db.Users
                       .Include(user => user.Roles)
                       .FirstOrDefaultAsync(user => user.Id == userId, ct) ??
                   throw new UnAuthorizedException("Invalid user.");

        return new GetCurrentUserResult(
            userId,
            user.Email,
            user.FirstName,
            user.LastName,
            user.Roles.ToArray().Select(r => r.Name)
        );
    }
}