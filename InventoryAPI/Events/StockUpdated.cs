using InventoryAPI.Models.Enums;
using MediatR;

namespace InventoryAPI.Events;

public record StockUpdated(Guid InventoryId, int Quantity, InventoryAction Action) : INotification;