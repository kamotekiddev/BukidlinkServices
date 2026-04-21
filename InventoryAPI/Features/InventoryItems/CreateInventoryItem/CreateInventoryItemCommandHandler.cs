using InventoryAPI.Common.Exceptions;
using InventoryAPI.Infrastructure;
using InventoryAPI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Features.InventoryItems.CreateInventoryItem;

public class CreateInventoryItemCommandHandler(AppDbContext dbContext)
    : IRequestHandler<CreateInventoryItemCommand, InventoryItem>
{
    public async Task<InventoryItem> Handle(CreateInventoryItemCommand request, CancellationToken cancellationToken)
    {
        var existingInventoryItem =
            await dbContext.Inventories.FirstOrDefaultAsync(i => i.ProductVariantId == request.ProductVariantId,
                cancellationToken);

        if (existingInventoryItem != null) throw new ProductVariantAlreadyExistException(request.ProductVariantId);

        var productVariant = new InventoryItem
        {
            ProductVariantId = request.ProductVariantId,
            Quantity = request.Quantity,
        };

        dbContext.Inventories.Add(productVariant);

        await dbContext.SaveChangesAsync(cancellationToken);
        return productVariant;
    }
}