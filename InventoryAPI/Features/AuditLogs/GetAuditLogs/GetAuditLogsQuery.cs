using InventoryAPI.Models;
using MediatR;

namespace InventoryAPI.Features.AuditLogs.GetAuditLogs;

public record GetAuditLogsQuery(Guid? OrderId, Guid? InventoryId, InventoryAction? Action) : IRequest<AuditLog[]>;