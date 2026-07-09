namespace Auth.Domain;

public class User
{
    private User()
    {
    }

    public Guid Id { get; init; }
    public string Email { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string HashedPassword { get; private set; }
    public DateTime? VerifiedAt { get; set; }
    public bool IsVerified => VerifiedAt is not null;
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public static User Create(string email, string firstName, string lastName, string hashedPassword)
    {
        return new User
        {
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            HashedPassword = hashedPassword
        };
    }

    public void VerifyUser()
    {
        if (VerifiedAt is not null)
            return;

        VerifiedAt = DateTime.UtcNow;
    }
}