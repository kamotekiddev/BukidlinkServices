using System.Text.Json;
using InventoryAPI.Infrastructure;
using InventoryAPI.Models;
using MediatR;

namespace InventoryAPI.Features.AuditLogs.StockReservedEvent;

public class StockReservedEventHandler(AppDbContext dbContext) : INotificationHandler<StockReservedEvent>
{
    public async Task Handle(StockReservedEvent notification, CancellationToken ct)
    {
        var reserveLog = new AuditLog
        {
            Action = nameof(InventoryAction.Reserve),
            EntityId = notification.InventoryItemId,
            EntityName = nameof(Inventory),
            OrderId = notification.OrderId,
            Data = JsonSerializer.Serialize(new { ReservedQuantity = notification.Quantity }),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        dbContext.AuditLogs.Add(reserveLog);
        await dbContext.SaveChangesAsync(ct);
    }
}