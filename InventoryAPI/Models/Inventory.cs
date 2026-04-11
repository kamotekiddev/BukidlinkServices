namespace InventoryAPI.Models;

public class Inventory
{
    public Guid Id { get; set; }
    public Guid ProductVariantId { get; set; }
    public int Quantity { get; set; }
    public int ReservedQuantity { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}