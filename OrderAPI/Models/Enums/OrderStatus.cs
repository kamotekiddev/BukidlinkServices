namespace OrderAPI.Models.Enums;

public enum OrderStatus
{
    Pending,
    Placed,
    Preparing,
    ReadyForPickup,
    Shipped,
    Delivered,
    Completed,
    Cancelled
}