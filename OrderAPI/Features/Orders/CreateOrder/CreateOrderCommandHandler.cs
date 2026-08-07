using BuildingBlocks.Auth;
using BuildingBlocks.Contracts.Product;
using BuildingBlocks.Exceptions;
using MediatR;
using OrderAPI.Infrastructure;
using OrderAPI.Infrastructure.HttpClients.ProductServiceClient;
using OrderAPI.Infrastructure.HttpClients.StoreServiceClient;
using OrderAPI.Models;

namespace OrderAPI.Features.Orders.CreateOrder;

public class CreateOrderCommandHandler(
    AppDbContext db,
    ILogger<CreateOrderCommandHandler> logger,
    ICurrentUser currentUser,
    IProductServiceClient productServiceClient,
    IStoreServiceClient storeServiceClient
)
    : IRequestHandler<CreateOrderCommand, Order>
{
    public async Task<Order> Handle(CreateOrderCommand request, CancellationToken ct)
    {
        var userId = currentUser.UserId ?? throw new BadRequestException("Unauthenticated.");
        var variantIds = request.OrderItems.Select(item => item.ProductVariantId).ToArray();

        logger.LogInformation(
            "Creating order for StoreId:{StoreId}, ProductVariantIds:{VariantIds}",
            request.StoreId,
            string.Join(",", variantIds)
        );

        var storeTask = storeServiceClient.GetStoreByIdAsync(request.StoreId, ct);
        var variantsTask = productServiceClient.GetProductVariantsByIds(variantIds, ct);

        await Task.WhenAll(storeTask, variantsTask);

        await storeTask;
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

        if (mismatchedVariantIds.Any())
        {
            logger.LogError(
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

        db.Orders.Add(order);
        await db.SaveChangesAsync(ct);

        logger.LogInformation(
            "Order created successfully. OrderId: {OrderId}, UserId: {UserId}, StoreId: {StoreId}, VariantIds: {VariantIds}",
            order.Id,
            userId,
            request.StoreId,
            string.Join(",", variantIds)
        );

        // TODO: reserve the stock after order is created

        return order;
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
}