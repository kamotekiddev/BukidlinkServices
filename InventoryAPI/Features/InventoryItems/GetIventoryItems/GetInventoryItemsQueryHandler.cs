using InventoryAPI.Infrastructure;
using InventoryAPI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Features.InventoryItems.GetIventoryItems
{
    public class GetInventoryItemsQueryHandler(AppDbContext dbContext) : IRequestHandler<GetInventoryItemsQuery, ICollection<InventoryItem>>
    {
        public async Task<ICollection<InventoryItem>> Handle(GetInventoryItemsQuery request, CancellationToken cancellationToken)
        {
            return await dbContext.Inventories.ToListAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
