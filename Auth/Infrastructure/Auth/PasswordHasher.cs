namespace Auth.Infrastructure.Auth;

public class PasswordHasher : IPasswordHasher
{
    public Task<string> HashPassword(string password)
    {
        throw new NotImplementedException();
    }
}