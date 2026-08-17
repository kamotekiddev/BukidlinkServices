using BuildingBlocks.Auth;
using BuildingBlocks.Contracts;
using BuildingBlocks.Contracts.Product;
using BuildingBlocks.Exceptions;
using MassTransit;
using MediatR;
using OrderAPI.Infrastructure;
using OrderAPI.Infrastructure.HttpClients.InventoryServiceClient;
using OrderAPI.Infrastructure.HttpClients.InventoryServiceClient.Models;
using OrderAPI.Infrastructure.HttpClients.ProductServiceClient;
using OrderAPI.Infrastructure.HttpClients.StoreServiceClient;
using OrderAPI.Models;
using OrderAPI.Models.Enums;

namespace OrderAPI.Features.Orders.CreateOrder;

public class CreateOrderCommandHandler(
    AppDbContext db,
    ILogger<CreateOrderCommandHandler> logger,
    ICurrentUser currentUser,
    IProductServiceClient productServiceClient,
    IStoreServiceClient storeServiceClient,
    IInventoryServiceClient inventoryServiceClient,
    IPublishEndpoint publisher
)
    : IRequestHandler<CreateOrderCommand, Order>
{
    public async Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId ?? throw new BadRequestException("Unauthenticated.");
        var variantIds = request.OrderItems.Select(item => item.ProductVariantId).ToArray();

        logger.LogInformation(
            "Creating order for StoreId:{StoreId}, ProductVariantIds:{VariantIds}",
            request.StoreId,
            string.Join(",", variantIds)
        );

        var duplicateVariantIds = GetDuplicateVariantIds(variantIds);
        if (duplicateVariantIds.Count != 0)
        {
            logger.LogWarning(
                "Encountered duplicate variants during order validation. Duplicates:{Duplicates}",
                string.Join(",", duplicateVariantIds)
            );

            throw new BadRequestException(
                $"Encountered duplicate variant ids during the order validation. Duplicates:{string.Join(",", duplicateVariantIds)}"
            );
        }

        var storeTask = EnsureStoreExistAsync(request.StoreId);
        var variantsTask = productServiceClient.GetProductVariantsByIds(variantIds, cancellationToken);

        await Task.WhenAll(storeTask, variantsTask);

        var productVariants = await variantsTask;

        if (variantIds.Length != productVariants.Count)
        {
            logger.LogWarning(
                "Some variant ids are invalid. out of {VariantIdsLength} only {ProductVariantsCount} are valid.",
                variantIds.Length,
                productVariants.Count
            );

            throw new BadRequestException(
                $"Some variant ids are invalid. out of {variantIds.Length} only {productVariants.Count} are valid."
            );
        }

        var mismatchedVariantIds = GetMismatchedVariantIds(request.StoreId, productVariants);

        if (mismatchedVariantIds.Count > 0)
        {
            logger.LogWarning(
                "Variant Ids provided :{MismatchedVariantIds} does not belong to store: {StoreId}.",
                string.Join(",", mismatchedVariantIds),
                request.StoreId
            );

            throw new BadRequestException(
                $"Variant Ids provided :{string.Join(",", mismatchedVariantIds)} does not belong to store: {request.StoreId}."
            );
        }

        var orderItems = MapOrderItemDtoToOrderItems(productVariants, request.OrderItems);

        var order = Order.Create(
            userId,
            request.StoreId,
            request.PaymentMethod,
            orderItems
        );

        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

        db.Orders.Add(order);
        await db.SaveChangesAsync(cancellationToken);

        var reservationItems = orderItems
            .Select(item => new ReservationItem(item.ProductVariantId, item.Quantity))
            .ToList();

        var inventoryReserved = false;

        try
        {
            await inventoryServiceClient.ReserveStocksForVariants(order.Id, reservationItems, cancellationToken);
            inventoryReserved = true;

            order.Place();
            await db.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }

        catch (Exception ex)
        {
            if (inventoryReserved)
            {
                logger.LogError(ex,
                    "Order failed. Inventory are reserved but failed to process order. Releasing the reservation for OrderId:{OrderId}",
                    order.Id
                );

                await publisher.Publish(new ReleaseStockEvent(order.Id), cancellationToken);
            }

            logger.LogError(ex, "Order failed.");
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }

        if (request.PaymentMethod != PaymentMethod.CashOnDelivery)
        {
            // initiate payment
        }

        logger.LogInformation(
            "Order created successfully. OrderId: {OrderId}, UserId: {UserId}, StoreId: {StoreId}, VariantIds: {VariantIds}",
            order.Id,
            userId,
            request.StoreId,
            string.Join(",", variantIds)
        );


        return order;
    }

    private async Task EnsureStoreExistAsync(Guid storeId)
    {
        await storeServiceClient.GetStoreByIdAsync(storeId);
    }

    private static IReadOnlyList<Guid> GetMismatchedVariantIds(
        Guid storeId,
        IReadOnlyList<ProductVariantDto> productVariants
    )
    {
        return productVariants
            .Where(variant => variant.StoreId != storeId)
            .Select(variant => variant.VariantId)
            .ToList();
    }

    private static List<OrderItem> MapOrderItemDtoToOrderItems(
        IReadOnlyList<ProductVariantDto> productVariants,
        IReadOnlyList<OrderItemDto> orderItems
    )
    {
        var productVariantLookup = productVariants.ToDictionary(v => v.VariantId);

        return orderItems
            .Select(orderItem =>
            {
                var variant = productVariantLookup[orderItem.ProductVariantId];
                return OrderItem.Create(variant.VariantId, orderItem.Quantity, variant.Price);
            })
            .ToList();
    }

    private static ICollection<Guid> GetDuplicateVariantIds(ICollection<Guid> variantIds)
    {
        return variantIds
            .GroupBy(id => id)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .ToList();
    }
}