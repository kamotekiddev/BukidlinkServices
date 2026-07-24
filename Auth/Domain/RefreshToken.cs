namespace Auth.Domain;

public class RefreshToken
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string Token { get; init; }

    public DateTime ExpiresAt { get; init; }
    public DateTime? RevokedAt { get; private set; }

    public User User { get; set; }

    public void Revoke()
    {
        if (RevokedAt is not null) return;
        RevokedAt = DateTime.UtcNow;
    }
}