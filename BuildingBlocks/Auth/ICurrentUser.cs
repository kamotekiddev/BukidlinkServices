namespace BuildingBlocks.Auth;

public interface ICurrentUser
{
    Guid? UserId { get; }
    string? Email { get; }
    IReadOnlyCollection<string> Roles { get; }
}