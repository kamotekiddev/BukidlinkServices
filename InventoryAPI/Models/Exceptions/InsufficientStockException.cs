using BuildingBlocks.Contracts.Inventory;
using BuildingBlocks.Exceptions;

namespace InventoryAPI.Models.Exceptions;

public sealed class InsufficientStockException : DomainException
{
    public InsufficientStockException()
        : base(
            StatusCodes.Status409Conflict,
            "Not enough stock available.",
            InventoryErrorCodes.InsufficientStock
        )
    {
    }
}