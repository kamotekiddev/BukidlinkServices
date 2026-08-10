using BuildingBlocks.Entities;
using InventoryAPI.Models.Enums;

namespace InventoryAPI.Models;

public class AuditLog : Entity
{
    public required InventoryAction Action { get; init; }
    public required Guid EntityId { get; init; }
    public required string EntityName { get; init; }
    public Guid? OrderId { get; init; }

    public string? Data { get; init; }
}