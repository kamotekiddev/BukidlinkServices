using MediatR;
using InventoryAPI.Models;

namespace InventoryAPI.Features.Inventories.GetInventories
{
    public record GetInventoriesQuery() : IRequest<ICollection<Inventory>>;
}
