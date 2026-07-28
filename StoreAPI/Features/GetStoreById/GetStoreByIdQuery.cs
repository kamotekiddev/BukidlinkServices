using BuildingBlocks.Contracts.Store;
using MediatR;

namespace StoreAPI.Features.GetStoreById;

public record GetStoreByIdQuery(Guid StoreId) : IRequest<StoreDto>;