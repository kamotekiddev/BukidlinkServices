namespace InventoryAPI.Models;

public class InventoryReservation : BaseEntity
{
    public Guid Id { get; init; }
    public required Guid InventoryId { get; init; }
    public required Guid OrderId { get; init; }
    public required int Quantity { get; init; }
}