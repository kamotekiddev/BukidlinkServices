using MediatR;

namespace StoreAPI.Features.CreateStore;

public record CreateStoreCommand(
    string Name,
    string? Description,
    string? AddressLine1,
    string? AddressLine2,
    string City,
    string Province,
    string Zipcode
)
    : IRequest<CreateStoreResult>;