using BuildingBlocks.Entities;
using OrderAPI.Exceptions;
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
    public PaymentMethod PaymentMethod { get; init; }
    public PaymentStatus PaymentStatus { get; private set; }

    public List<OrderItem> OrderItems { get; init; } = [];
    public List<OrderHistory> Histories { get; init; } = [];

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


        var order = new Order
        {
            UserId = userId,
            StoreId = storeId,
            OrderItems = orderItems,
            Status = OrderStatus.Pending,
            PaymentMethod = paymentMethod,
            PaymentStatus = PaymentStatus.Pending
        };

        order.Histories.Add(OrderHistory.Create(order.Id, OrderHistoryAction.Created));

        return order;
    }

    public void Place()
    {
        EnsureStatus(OrderStatus.Pending);

        ChangeStatus(OrderStatus.Placed);
    }

    public void StartPreparing()
    {
        EnsureStatus(OrderStatus.Placed);

        ChangeStatus(OrderStatus.Preparing);
    }

    public void MarkReadyForPickup()
    {
        EnsureStatus(OrderStatus.Preparing);

        ChangeStatus(OrderStatus.ReadyForPickup);
    }

    public void Ship()
    {
        if (Status != OrderStatus.Preparing && Status != OrderStatus.ReadyForPickup)
            throw new Exception("Only prepared orders can be shipped.");

        ChangeStatus(OrderStatus.Shipped);
    }

    public void Deliver()
    {
        EnsureStatus(OrderStatus.Shipped);

        Status = OrderStatus.Delivered;
        ChangeStatus(OrderStatus.Delivered);

        // COD payment happens upon delivery.
        if (PaymentMethod == PaymentMethod.CashOnDelivery)
        {
            ChangePaymentStatus(PaymentStatus.Paid);
        }
    }

    public void Complete()
    {
        EnsureStatus(OrderStatus.Delivered);

        ChangeStatus(OrderStatus.Completed);
    }

    public void Cancel()
    {
        if (Status is OrderStatus.Delivered or OrderStatus.Completed)
            throw new OrderCannotBeCancelledException(Status);

        if (Status is OrderStatus.Cancelled) return;

        ChangeStatus(OrderStatus.Cancelled);
    }

    public void MarkPaymentPaid()
    {
        if (PaymentStatus == PaymentStatus.Paid)
            throw new Exception("Payment has already been completed.");

        ChangePaymentStatus(PaymentStatus.Paid);
    }

    public void MarkPaymentFailed()
    {
        ChangePaymentStatus(PaymentStatus.Failed);
    }

    public void Refund()
    {
        if (PaymentStatus != PaymentStatus.Paid)
            throw new Exception("Only paid orders can be refunded.");

        ChangePaymentStatus(PaymentStatus.Refunded);
    }

    private void ChangeStatus(OrderStatus newStatus)
    {
        AddHistory(OrderHistoryAction.StatusChanged,
            Status.ToString(),
            newStatus.ToString()
        );

        Status = newStatus;
    }

    private void ChangePaymentStatus(PaymentStatus newStatus)
    {
        AddHistory(OrderHistoryAction.PaymentStatusChanged,
            Status.ToString(),
            newStatus.ToString()
        );

        PaymentStatus = newStatus;
    }

    private void AddHistory(OrderHistoryAction action, string? previousValue = null, string? newValue = null)
    {
        Histories.Add(OrderHistory.Create(Id, action, previousValue, newValue));
    }

    private void EnsureStatus(OrderStatus expected)
    {
        if (Status != expected)
            throw new InvalidOrderStatusException(Status);
    }
}