using InventoryAPI.Infrastructure;
using InventoryAPI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Features.Inventories.GetInventories
{
    public class GetInventoriesQueryHandler(AppDbContext dbContext)
        : IRequestHandler<GetInventoriesQuery, ICollection<Inventory>>
    {
        public async Task<ICollection<Inventory>> Handle(GetInventoriesQuery request,
            CancellationToken cancellationToken)
        {
            return await dbContext.Inventories
                .Include(inventoryItems => inventoryItems.Reservations)
                .ToListAsync(cancellationToken);
        }
    }
}