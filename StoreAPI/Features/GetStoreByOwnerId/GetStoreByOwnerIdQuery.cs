using MediatR;

namespace StoreAPI.Features.GetStoreByOwnerId;

public record GetStoreByOwnerIdQuery(Guid OwnerId) : IRequest<GetStoreByOwnerIdResult>;