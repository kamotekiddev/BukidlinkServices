using InventoryAPI.Models;
using MediatR;

namespace InventoryAPI.Events;

public record StockUpdated(Guid InventoryId, int Quantity, InventoryAction Action) : INotification;