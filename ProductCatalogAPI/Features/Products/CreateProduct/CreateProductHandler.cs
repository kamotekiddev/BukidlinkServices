using BuildingBlocks.Auth;
using BuildingBlocks.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Common.Exceptions;
using ProductCatalogAPI.Domain;
using ProductCatalogAPI.Infrastructure;
using ProductCatalogAPI.Infrastructure.HttpClients.StoreHttpClient;

namespace ProductCatalogAPI.Features.Products.CreateProduct;

public class CreateProductHandler(
    AppDbContext db,
    ICurrentUser currentUser,
    StoreClient storeClient,
    ILogger<CreateProductHandler> logger
)
    : IRequestHandler<CreateProductCommand, Product>
{
    public async Task<Product> Handle(CreateProductCommand request, CancellationToken ct)
    {
        var userId = currentUser.UserId ?? throw new UnAuthorizedException("Unauthenticated.");

        logger.LogInformation(
            "Creating product '{RequestName}' for Store '{RequestStoreId}' by user '{UserId}'",
            request.Name,
            request.StoreId,
            userId
        );

        var existingStore = await storeClient.GetStoreById(request.StoreId, ct) ??
                            throw new NotFoundException("Store does not exist");

        if (existingStore.OwnerId != userId)
        {
            logger.LogWarning(
                "User '{UserId}' attempted to create product in store '{StoreId}' owned by '{OwnerId}'",
                userId,
                request.StoreId,
                existingStore.OwnerId
            );

            throw new BadRequestException("The store is not owned by the user.");
        }


        var existingProduct =
            await db.Products.FirstOrDefaultAsync(
                product =>
                    product.Name == request.Name &&
                    product.StoreId == request.StoreId, ct
            );


        if (existingProduct is not null)
        {
            logger.LogInformation(
                "Duplicate product '{ProductName}' attempted in store '{StoreId}'",
                request.Name,
                request.StoreId);

            throw new ProductAlreadyExistsException(request.Name);
        }

        var product = Product.Create(
            request.Name,
            request.Description,
            request.StoreId
        );

        db.Products.Add(product);
        await db.SaveChangesAsync(ct);

        logger.LogInformation(
            "Product '{ProductId}' created successfully in store '{StoreId}'",
            product.Id,
            product.StoreId);

        return product;
    }
}