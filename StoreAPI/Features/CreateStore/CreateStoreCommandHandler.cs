using BuildingBlocks.Auth;
using BuildingBlocks.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreAPI.Domain;
using StoreAPI.Domain.ValueObjects;
using StoreAPI.Infrastructure;

namespace StoreAPI.Features.CreateStore;

internal sealed class CreateStoreCommandHandler(
    AppDbContext db,
    ICurrentUser currentUser
)
    : IRequestHandler<CreateStoreCommand, CreateStoreResult>
{
    public async Task<CreateStoreResult> Handle(CreateStoreCommand request, CancellationToken ct)
    {
        var userId = currentUser.UserId ?? throw new UnAuthorizedException("User not authenticated.");

        var existingStore = await db.Stores.FirstOrDefaultAsync(store => store.Name == request.Name, ct);
        if (existingStore != null)
            throw new BadRequestException("Store name already been used.");

        var storeAddress = Address.Create(
            request.AddressLine1,
            request.AddressLine2,
            request.City,
            request.Province,
            request.Zipcode
        );

        var store = Store.Create(
            request.Name,
            request.Description,
            userId,
            storeAddress
        );

        db.Stores.Add(store);
        await db.SaveChangesAsync(ct);

        return new CreateStoreResult(store.Id);
    }
}