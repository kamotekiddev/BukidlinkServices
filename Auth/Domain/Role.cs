namespace Auth.Domain;

public class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}