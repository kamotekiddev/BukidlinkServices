using InventoryAPI.Models;
using MediatR;

namespace InventoryAPI.Features.InventoryItems.CreateInventoryItem;

public record CreateInventoryItemCommand(Guid ProductVariantId, int Quantity)
    : IRequest<InventoryItem>;