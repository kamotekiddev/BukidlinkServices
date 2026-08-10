using System.Text.Json;
using InventoryAPI.Events;
using InventoryAPI.Infrastructure;
using InventoryAPI.Models;
using InventoryAPI.Models.Enums;
using MediatR;

namespace InventoryAPI.Features.AuditLogs.Events;

public class StockReservedAuditHandler(AppDbContext dbContext) : INotificationHandler<StockReserved>
{
    public async Task Handle(StockReserved notification, CancellationToken ct)
    {
        var reserveLog = new AuditLog
        {
            Action = InventoryAction.Reserve,
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