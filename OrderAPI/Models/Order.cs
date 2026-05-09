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
    public OrderStatus Status { get; private set; }
    public List<OrderItem> OrderItems { get; private set; } = new();

    public decimal TotalPrice => OrderItems.Sum(item => item.Quantity * item.SellPrice);
    
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    private Order()
    {
    }

    public static Order Create(Guid userId, List<OrderItem> orderItems)
    {
        if (userId == Guid.Empty) throw new Exception("User Id cannot be empty.");
        if (orderItems.Count == 0) throw new Exception("Order must have at least one item.");

        return new Order
        {
            UserId = userId,
            OrderItems = orderItems,
            Status = OrderStatus.Placed
        };
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Confirmed) throw new Exception("Confirmed orders cannot be cancelled.");

        Status = OrderStatus.Cancelled;
    }

    public void Confirm()
    {
        if (Status == OrderStatus.Cancelled) throw new Exception("Cancelled orders cannot be confirmed");

        Status = OrderStatus.Confirmed;
    }
}