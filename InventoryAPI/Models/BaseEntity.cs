namespace InventoryAPI.Models;

public class BaseEntity
{
    public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}