namespace BuildingBlocks.Contracts.Store;

public record class StoreDto(
    Guid StoreId,
    Guid OwnerId,
    string Name
);