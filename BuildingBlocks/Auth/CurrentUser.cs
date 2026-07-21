using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Auth;

public class CurrentUser(IHttpContextAccessor accessor) : ICurrentUser
{
    private readonly ClaimsPrincipal? _user = accessor.HttpContext?.User;

    public Guid? UserId =>
        Guid.TryParse(_user?.FindFirstValue(ClaimTypes.NameIdentifier), out var userId) ? userId : null;

    public string? Email => _user?.FindFirstValue(ClaimTypes.Email);
    public IReadOnlyCollection<string> Roles => _user?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray() ?? [];
}