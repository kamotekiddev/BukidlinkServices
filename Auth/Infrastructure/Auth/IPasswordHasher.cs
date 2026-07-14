namespace Auth.Infrastructure.Auth;

public interface IPasswordHasher
{
    Task<string> HashPassword(string password);
}