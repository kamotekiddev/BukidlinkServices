using BuildingBlocks.Entities;

namespace InventoryAPI.Models;

public class InventoryReservation : Entity
{
    public required Guid InventoryId { get; init; }
    public required Guid OrderId { get; init; }
    public required int Quantity { get; init; }
}