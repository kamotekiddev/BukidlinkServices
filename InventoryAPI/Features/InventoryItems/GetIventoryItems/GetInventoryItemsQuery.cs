using InventoryAPI.Models;
using MediatR;

namespace InventoryAPI.Features.InventoryItems.GetIventoryItems
{
    public record GetInventoryItemsQuery() : IRequest<ICollection<InventoryItem>>;
}
