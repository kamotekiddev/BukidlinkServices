using BuildingBlocks.Entities;
using OrderAPI.Models.Enums;

namespace OrderAPI.Models;

public class Order : Entity
{
    private Order()
    {
    }

    public Guid UserId { get; init; }

    public Guid StoreId { get; init; }

    public OrderStatus Status { get; private set; }

    public PaymentMethod PaymentMethod { get; private set; }

    public PaymentStatus PaymentStatus { get; private set; }

    public List<OrderItem> OrderItems { get; private set; } = [];

    public decimal TotalPrice =>
        OrderItems.Sum(item => item.Quantity * item.SellPrice);

    public static Order Create(
        Guid userId,
        Guid storeId,
        PaymentMethod paymentMethod,
        List<OrderItem> orderItems)
    {
        if (userId == Guid.Empty)
            throw new Exception("User Id cannot be empty.");

        if (storeId == Guid.Empty)
            throw new Exception("Store Id cannot be empty.");

        if (orderItems.Count == 0)
            throw new Exception("Order must contain at least one item.");

        return new Order
        {
            UserId = userId,
            StoreId = storeId,
            OrderItems = orderItems,
            Status = OrderStatus.Pending,
            PaymentMethod = paymentMethod,
            PaymentStatus = PaymentStatus.Pending
        };
    }

    public void Place()
    {
        EnsureStatus(OrderStatus.Pending);

        Status = OrderStatus.Placed;
    }

    public void StartPreparing()
    {
        EnsureStatus(OrderStatus.Placed);

        Status = OrderStatus.Preparing;
    }

    public void MarkReadyForPickup()
    {
        EnsureStatus(OrderStatus.Preparing);

        Status = OrderStatus.ReadyForPickup;
    }

    public void Ship()
    {
        if (Status != OrderStatus.Preparing && Status != OrderStatus.ReadyForPickup)
            throw new Exception("Only prepared orders can be shipped.");

        Status = OrderStatus.Shipped;
    }

    public void Deliver()
    {
        EnsureStatus(OrderStatus.Shipped);

        Status = OrderStatus.Delivered;

        // COD payment happens upon delivery.
        if (PaymentMethod == PaymentMethod.CashOnDelivery)
            PaymentStatus = PaymentStatus.Paid;
    }

    public void Complete()
    {
        EnsureStatus(OrderStatus.Delivered);

        Status = OrderStatus.Completed;
    }

    public void Cancel()
    {
        if (Status is OrderStatus.Delivered or OrderStatus.Completed)
            throw new Exception("Completed or delivered orders cannot be cancelled.");

        Status = OrderStatus.Cancelled;
    }

    public void MarkPaymentPaid()
    {
        if (PaymentStatus == PaymentStatus.Paid)
            throw new Exception("Payment has already been completed.");

        PaymentStatus = PaymentStatus.Paid;
    }

    public void MarkPaymentFailed()
    {
        PaymentStatus = PaymentStatus.Failed;
    }

    public void Refund()
    {
        if (PaymentStatus != PaymentStatus.Paid)
            throw new Exception("Only paid orders can be refunded.");

        PaymentStatus = PaymentStatus.Refunded;
    }

    private void EnsureStatus(OrderStatus expected)
    {
        if (Status != expected)
            throw new Exception($"Expected order status '{expected}' but found '{Status}'.");
    }
}