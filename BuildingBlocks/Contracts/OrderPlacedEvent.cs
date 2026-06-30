namespace BuildingBlocks.Contracts;

public record OrderPlacedEvent(Guid OrderId, Guid UserId, string Status);