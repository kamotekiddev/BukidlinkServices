using InventoryAPI.Infrastructure;
using InventoryAPI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Features.InventoryItems.GetInventoryItems
{
    public class GetInventoryItemsQueryHandler(AppDbContext dbContext)
        : IRequestHandler<GetInventoryItemsQuery, ICollection<InventoryItem>>
    {
        public async Task<ICollection<InventoryItem>> Handle(GetInventoryItemsQuery request,
            CancellationToken cancellationToken)
        {
            return await dbContext.Inventories
                .Include(inventoryItems => inventoryItems.Reservations)
                .ToListAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}