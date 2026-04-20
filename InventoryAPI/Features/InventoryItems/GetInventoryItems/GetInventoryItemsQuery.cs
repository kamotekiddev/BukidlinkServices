using InventoryAPI.Models;
using MediatR;

namespace InventoryAPI.Features.InventoryItems.GetInventoryItems
{
    public record GetInventoryItemsQuery() : IRequest<ICollection<InventoryItem>>;
}
