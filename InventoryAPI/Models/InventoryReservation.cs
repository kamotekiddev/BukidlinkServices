using BuildingBlocks.Entities;

namespace InventoryAPI.Models;

public class InventoryReservation : Entity
{
    public Guid InventoryId { get; init; }
    public Guid OrderId { get; init; }
    public int Quantity { get; init; }

    public Inventory Inventory { get; init; }
}