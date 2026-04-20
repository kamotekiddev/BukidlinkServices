namespace InventoryAPI.Models;

public class InventoryItem
{
    public Guid Id { get; set; }
    public Guid ProductVariantId { get; set; }
    public int Quantity { get; set; }
    public int ReservedQuantity { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int Available => Quantity - ReservedQuantity;

    public void IncreaseQuantity(int count)
    {
        Quantity += count;
    }

    public void DecreaseQuantity(int count)
    {
        if (count > Available) throw new Exception("Not enough stock.");
        Quantity -= count;
    }
}