using BuildingBlocks.Entities;
using OrderAPI.Models.Enums;

namespace OrderAPI.Models;

public class OrderHistory : Entity
{
    private OrderHistory()
    {
    }

    public Guid OrderId { get; init; }
    public OrderHistoryAction Action { get; init; }
    public string? PreviousValue { get; private set; }
    public string? NewValue { get; private set; }

    public Order Order { get; init; }

    public static OrderHistory Create(Guid orderId,
        OrderHistoryAction action,
        string? previousValue = null,
        string? newValue = null
    )
    {
        return new OrderHistory
        {
            OrderId = orderId,
            Action = action,
            PreviousValue = previousValue,
            NewValue = newValue
        };
    }
}