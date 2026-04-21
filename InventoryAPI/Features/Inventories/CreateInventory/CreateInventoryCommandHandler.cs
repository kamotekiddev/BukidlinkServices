using InventoryAPI.Common.Exceptions;
using InventoryAPI.Infrastructure;
using InventoryAPI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Features.Inventories.CreateInventory;

public class CreateInventoryCommandHandler(AppDbContext dbContext)
    : IRequestHandler<CreateInventoryCommand, Inventory>
{
    public async Task<Inventory> Handle(CreateInventoryCommand request, CancellationToken cancellationToken)
    {
        var existingInventoryItem =
            await dbContext.Inventories.FirstOrDefaultAsync(i => i.ProductVariantId == request.ProductVariantId,
                cancellationToken);

        if (existingInventoryItem != null) throw new ProductVariantAlreadyExistException(request.ProductVariantId);

        var productVariant = new Inventory
        {
            ProductVariantId = request.ProductVariantId,
            Quantity = request.Quantity,
        };

        dbContext.Inventories.Add(productVariant);

        await dbContext.SaveChangesAsync(cancellationToken);
        return productVariant;
    }
}