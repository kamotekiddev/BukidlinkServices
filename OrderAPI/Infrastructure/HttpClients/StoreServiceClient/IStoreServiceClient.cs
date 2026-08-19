using BuildingBlocks.Contracts.Store;

namespace OrderAPI.Infrastructure.HttpClients.StoreServiceClient;

public interface IStoreServiceClient
{
    Task<StoreDto> GetStoreByIdAsync(Guid storeId, CancellationToken ct = default);
}