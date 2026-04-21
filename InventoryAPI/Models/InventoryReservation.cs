namespace InventoryAPI.Models;

public class InventoryReservation
{
    public Guid Id { get; init; }
    public Guid InventoryItemId { get; init; }
    public int Quantity { get; init; }
    
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}