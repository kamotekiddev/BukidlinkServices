using BuildingBlocks.Entities;
using BuildingBlocks.Exceptions;

namespace InventoryAPI.Models;

public class Inventory : Entity
{
    public Guid ProductVariantId { get; init; }
    public int Quantity { get; set; }

    public List<InventoryReservation> Reservations { get; init; } = [];

    public int ReservedQuantity { get; private set; }
    public int AvailableQuantity => Quantity - ReservedQuantity;

    public void IncreaseQuantity(int count)
    {
        Quantity += count;
    }

    public void DecreaseQuantity(int count)
    {
        if (count > AvailableQuantity)
            throw new Exception("Not enough stock.");
        Quantity -= count;
    }

    public void Reserve(int count, Guid orderId)
    {
        if (count > AvailableQuantity)
            throw new BadRequestException("Not enough stock.");

        ReservedQuantity += count;

        Reservations.Add(
            new InventoryReservation
            {
                InventoryId = Id,
                OrderId = orderId,
                Quantity = count
            });
    }
}