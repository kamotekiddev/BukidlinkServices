using Auth.Domain;

namespace Auth.Infrastructure.Auth;

public interface ITokenProvider
{
    string GenerateAccessToken(User user);
    RefreshToken GenerateRefreshToken(User user);
}