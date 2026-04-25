using System.Text.Json;
using InventoryAPI.Events;
using InventoryAPI.Infrastructure;
using InventoryAPI.Models;
using MediatR;

namespace InventoryAPI.Features.AuditLogs.Events;

public class StockUpdatedAuditHandler(AppDbContext dbContext) : INotificationHandler<StockUpdated>
{
    public async Task Handle(StockUpdated notification, CancellationToken ct)
    {
        var updateLog = new AuditLog
        {
            Action = notification.Action,
            EntityId = notification.InventoryId,
            EntityName = nameof(Inventory),
            OrderId = null,
            Data = JsonSerializer.Serialize(new { Quantity = notification.Quantity }),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        dbContext.AuditLogs.Add(updateLog);
        await dbContext.SaveChangesAsync(ct);
    }
}