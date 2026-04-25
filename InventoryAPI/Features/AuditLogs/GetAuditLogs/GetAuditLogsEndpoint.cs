using Carter;
using InventoryAPI.Models;
using MediatR;

namespace InventoryAPI.Features.AuditLogs.GetAuditLogs;

public class GetAuditLogsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/inventories/audit-logs",
            async (Guid? orderId, Guid? inventoryId, InventoryAction? action, IMediator sender) =>
            {
                var inventoryAuditLogs = await sender.Send(new GetAuditLogsQuery(orderId, inventoryId, action));
                return Results.Ok(inventoryAuditLogs);
            });
    }
}