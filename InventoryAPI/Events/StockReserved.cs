using MediatR;

namespace InventoryAPI.Events;

public record StockReserved(
    Guid InventoryItemId,
    Guid? OrderId,
    int Quantity,
    DateTime OccurredAt
) : INotification;