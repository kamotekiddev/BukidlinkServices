using MediatR;

namespace InventoryAPI.Features.AuditLogs.StockReservedEvent;

public record StockReservedEvent(
    Guid InventoryItemId,
    Guid? OrderId,
    int Quantity,
    DateTime OccurredAt
) : INotification;