namespace OrderAPI.Models;

public enum OrderStatus
{
    Placed,
    Confirmed,
    Cancelled
}

public class Order
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public Guid ProductVariantId { get; init; }
    public OrderStatus Status { get; private set; }
    public int Quantity { get; init; }
    public DateTime CreatedAt { get; init; }

    public static Order Create(Guid userId, Guid productVariantId, int quantity)
    {
        return new Order
            { UserId = userId, ProductVariantId = productVariantId, Quantity = quantity, Status = OrderStatus.Placed };
    }

    public void UpdateStatus(OrderStatus status)
    {
        Status = status;
    }

    public void Cancel()
    {
        Status = OrderStatus.Cancelled;
    }

    public void Confirm()
    {
        Status = OrderStatus.Confirmed;
    }
}