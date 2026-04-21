namespace InventoryAPI.Models;

public class InventoryReservation : BaseEntity
{
    public Guid Id { get; init; }
    public Guid InventoryItemId { get; init; }
    public int Quantity { get; init; }
}