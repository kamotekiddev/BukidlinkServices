using MediatR;

namespace InventoryAPI.Features.Inventories.ReserveStocks;

public record ReservationItem(Guid ProductVariantId, int Quantity);

public record ReserveStocksCommand(
    Guid OrderId,
    ICollection<ReservationItem> Items
) : IRequest;