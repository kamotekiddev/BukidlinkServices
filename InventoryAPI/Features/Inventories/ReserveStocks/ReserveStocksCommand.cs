using InventoryAPI.Models;
using MediatR;

namespace InventoryAPI.Features.Inventories.ReserveStocks;

public record ReserveRequest(Guid ProductVariantId, int Quantity);

public record ReserveStocksCommand(
    Guid OrderId,
    IEnumerable<ReserveRequest> ReserveRequests
) : IRequest<ICollection<Inventory>>;