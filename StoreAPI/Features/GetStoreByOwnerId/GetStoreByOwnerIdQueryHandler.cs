using BuildingBlocks.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreAPI.Infrastructure;

namespace StoreAPI.Features.GetStoreByOwnerId;

public sealed class GetStoreByOwnerIdQueryHandler(AppDbContext db)
    : IRequestHandler<GetStoreByOwnerIdQuery, GetStoreByOwnerIdResult>
{
    public async Task<GetStoreByOwnerIdResult> Handle(GetStoreByOwnerIdQuery request, CancellationToken ct)
    {
        var store = await db.Stores.SingleOrDefaultAsync(store => store.OwnerId == request.OwnerId, ct) ??
                    throw new NotFoundException("Store does not exist.");

        return new GetStoreByOwnerIdResult(store.Id);
    }
}