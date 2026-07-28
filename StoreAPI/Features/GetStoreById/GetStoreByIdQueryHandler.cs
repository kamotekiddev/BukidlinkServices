using BuildingBlocks.Contracts.Store;
using BuildingBlocks.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreAPI.Infrastructure;

namespace StoreAPI.Features.GetStoreById;

public sealed class GetStoreByIdQueryHandler(AppDbContext db)
    : IRequestHandler<GetStoreByIdQuery, StoreDto>
{
    public async Task<StoreDto> Handle(GetStoreByIdQuery request, CancellationToken ct)
    {
        var store = await db.Stores.SingleOrDefaultAsync(store => store.Id == request.StoreId, ct) ??
                    throw new NotFoundException("Store does not exist.");

        return new StoreDto(store.Id, store.OwnerId, store.Name);
    }
}