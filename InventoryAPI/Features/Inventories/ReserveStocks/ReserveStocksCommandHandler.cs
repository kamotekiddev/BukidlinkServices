using BuildingBlocks.Exceptions;
using InventoryAPI.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Features.Inventories.ReserveStocks;

public class ReserveStocksCommandHandler(
    AppDbContext db,
    ILogger<ReserveStocksCommandHandler> logger)
    : IRequestHandler<ReserveStocksCommand>
{
    public async Task Handle(ReserveStocksCommand request, CancellationToken ct)
    {
        await using var transaction = await db.Database.BeginTransactionAsync(ct);

        var variantIds = request.ReserveRequests
            .Select(r => r.ProductVariantId)
            .Distinct()
            .ToList();

        logger.LogDebug(
            "Attempting to reserve stock. OrderId: {OrderId}, VariantIds: {VariantIds}",
            request.OrderId,
            variantIds
        );

        var inventories = await db.Inventories
            .FromSqlInterpolated($"""
                                  SELECT *
                                  FROM "Inventories"
                                  WHERE "ProductVariantId" = ANY({variantIds})
                                  ORDER BY "ProductVariantId"
                                  FOR UPDATE
                                  """)
            .ToListAsync(ct);

        if (inventories.Count != variantIds.Count)
        {
            logger.LogWarning(
                "Inventory not found for one or more product variants. OrderId: {OrderId}, VariantIds: {VariantIds}",
                request.OrderId,
                variantIds
            );

            throw new BadRequestException("One or more product variants do not have inventory.");
        }

        logger.LogDebug(
            "Inventory rows locked. OrderId: {OrderId}, VariantIds: {VariantIds}",
            request.OrderId,
            variantIds
        );

        var inventoriesLookup = inventories.ToDictionary(i => i.ProductVariantId);

        foreach (var req in request.ReserveRequests)
        {
            var inventory = inventoriesLookup[req.ProductVariantId];
            inventory.Reserve(req.Quantity, request.OrderId);
        }

        await db.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);

        logger.LogInformation(
            "Successfully reserved stock. OrderId: {OrderId}, Reservations: {@Reservations}",
            request.OrderId,
            request.ReserveRequests
        );
    }
}