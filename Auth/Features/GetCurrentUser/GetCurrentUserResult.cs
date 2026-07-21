namespace Auth.Features.GetCurrentUser;

public record GetCurrentUserResult(
    Guid UserId,
    string Email,
    string Firstname,
    string Lastname,
    IEnumerable<string> Roles
);