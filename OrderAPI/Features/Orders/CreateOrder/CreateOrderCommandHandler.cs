using BuildingBlocks.Auth;
using BuildingBlocks.Contracts.Product;
using BuildingBlocks.Exceptions;
using MediatR;
using OrderAPI.Infrastructure;
using OrderAPI.Infrastructure.HttpClients.ProductServiceClient;
using OrderAPI.Models;

namespace OrderAPI.Features.Orders.CreateOrder;

public class CreateOrderCommandHandler(
    AppDbContext db,
    ILogger<CreateOrderCommandHandler> logger,
    ICurrentUser currentUser,
    IProductServiceClient productServiceClient
)
    : IRequestHandler<CreateOrderCommand, Order>
{
    public async Task<Order> Handle(CreateOrderCommand request, CancellationToken ct)
    {
        // check product variants, get info, active, price, storeid

        var userId = currentUser.UserId ?? throw new BadRequestException("Unauthenticated.");
        var variantIds = request.OrderItems.Select(item => item.ProductVariantId).ToArray();

        logger.LogInformation(
            "Fetching product variants. StoreId: {StoreId}, VariantCount: {VariantCount}, VariantIds: {VariantIds}",
            request.StoreId,
            variantIds.Length,
            string.Join(", ", variantIds)
        );

        var productVariants = await productServiceClient.GetProductVariantsByIds(variantIds);

        if (variantIds.Length != productVariants.Count)
        {
            logger.LogWarning(
                "Product variant validation failed. StoreId: {StoreId}, RequestedCount: {RequestedCount}, RetrievedCount: {RetrievedCount}, VariantIds: {VariantIds}",
                request.StoreId,
                variantIds.Length,
                productVariants.Count,
                string.Join(", ", variantIds)
            );

            throw new BadRequestException(
                $"Some or all variant ids are invalid. VariantIds: {string.Join(",", variantIds)}");
        }

        var invalidVariantIds = GetInvalidVariantIds(request.StoreId, productVariants);

        if (invalidVariantIds.Any())
            throw new BadRequestException(
                $"Variant Ids provided :{string.Join(",", invalidVariantIds)} does not belong to store: {request.StoreId}."
            );

        var productVariantLookup = productVariants.ToDictionary(v => v.VariantId);

        var orderItems = request.OrderItems.Select(orderItem =>
            {
                var variant = productVariantLookup[orderItem.ProductVariantId];
                return OrderItem.Create(variant.VariantId, orderItem.Quantity, variant.Price);
            })
            .ToList();


        logger.LogInformation(
            "Creating order. UserId: {UserId}, StoreId: {StoreId}, ItemCount: {ItemCount}",
            userId,
            request.StoreId,
            orderItems.Count
        );

        var order = Order.Create(
            userId,
            request.StoreId,
            orderItems
        );

        db.Orders.Add(order);
        await db.SaveChangesAsync(ct);

        logger.LogInformation(
            "Order created successfully. OrderId: {OrderId}, UserId: {UserId}, StoreId: {StoreId}, ItemCount: {ItemCount}",
            order.Id,
            userId,
            request.StoreId,
            orderItems.Count
        );

        return order;
    }

    private static IReadOnlyList<Guid> GetInvalidVariantIds(Guid storeId,
        IReadOnlyList<ProductVariantDto> productVariants)
    {
        return productVariants
            .Where(variant => variant.StoreId != storeId)
            .Select(variant => variant.VariantId)
            .ToList();
    }
}