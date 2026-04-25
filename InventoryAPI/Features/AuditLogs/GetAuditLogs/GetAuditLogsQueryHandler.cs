using InventoryAPI.Infrastructure;
using InventoryAPI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Features.AuditLogs.GetAuditLogs;

public class GetAuditLogsQueryHandler(AppDbContext dbContext) : IRequestHandler<GetAuditLogsQuery, AuditLog[]>
{
    public async Task<AuditLog[]> Handle(GetAuditLogsQuery request, CancellationToken ct)
    {
        var query = dbContext.AuditLogs.AsQueryable();

        if (request.OrderId.HasValue)
        {
            query = query.Where(x => x.OrderId == request.OrderId);
        }

        if (request.InventoryId.HasValue)
        {
            query = query.Where(x => x.EntityId == request.InventoryId);
        }

        if (request.Action.HasValue)
        {
            query = query.Where(x => x.Action == request.Action);
        }

        return await query.ToArrayAsync(ct);
    }
}