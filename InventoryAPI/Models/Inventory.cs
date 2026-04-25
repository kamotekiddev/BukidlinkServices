namespace InventoryAPI.Models;

public class Inventory : BaseEntity
{
    public Guid Id { get; init; }
    public Guid ProductVariantId { get; init; }
    public int Quantity { get; set; }

    public List<InventoryReservation> Reservations { get; init; } = [];

    public int ReservedQuantity => Reservations.Sum(reservation => reservation.Quantity);
    public int AvailableQuantity => Quantity - ReservedQuantity;

    public void IncreaseQuantity(int count)
    {
        Quantity += count;
    }

    public void DecreaseQuantity(int count)
    {
        if (count > AvailableQuantity) throw new Exception("Not enough stock.");
        Quantity -= count;
    }

    public void Reserve(int count, Guid orderId)
    {
        if (count > AvailableQuantity) throw new Exception("Not enough stock.");
        Reservations.Add(new InventoryReservation { InventoryId = Id, OrderId = orderId, Quantity = count });
    }
}