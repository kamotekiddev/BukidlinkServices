namespace InventoryAPI.Models;

public class AuditLog : BaseEntity
{
    public Guid Id { get; init; }
    public required string Action { get; init; }
    public required Guid EntityId { get; init; }
    public required string EntityName { get; init; }
    public Guid? OrderId { get; init; }

    public string? Data { get; init; }
}