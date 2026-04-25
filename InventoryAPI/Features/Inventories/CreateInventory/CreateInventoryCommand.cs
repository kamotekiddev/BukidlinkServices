using MediatR;

namespace InventoryAPI.Features.Inventories.CreateInventory;

public record CreateInventoryCommand(Guid ProductVariantId, int Quantity)
    : IRequest<Models.Inventory>;