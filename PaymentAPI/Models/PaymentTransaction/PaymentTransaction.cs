using BuildingBlocks.Entities;
using BuildingBlocks.Exceptions;

namespace PaymentAPI.Models.PaymentTransaction;

public class PaymentTransaction : Entity
{
    private PaymentTransaction()
    {
    }

    public string ReferenceId { get; init; } = null!;
    public PaymentType Type { get; init; }
    public PaymentStatus Status { get; private set; }
    public decimal Amount { get; init; }

    public Guid? OrderId { get; init; }
    public Guid? StoreId { get; init; }
    public Guid? PaymentTransactionId { get; init; }

    public static PaymentTransaction CreatePayment(
        Guid orderId,
        string referenceId,
        decimal amount)
    {
        if (orderId == Guid.Empty)
            throw new DomainException("Invalid OrderId.");

        return Create(
            referenceId,
            PaymentType.Payment,
            amount,
            orderId);
    }

    public static PaymentTransaction CreateRefund(
        Guid originalPaymentId,
        string referenceId,
        decimal amount)
    {
        if (originalPaymentId == Guid.Empty)
            throw new DomainException("Invalid original payment ID.");

        return Create(
            referenceId,
            PaymentType.Refund,
            amount,
            originalPaymentId: originalPaymentId);
    }

    public static PaymentTransaction CreatePayout(
        Guid storeId,
        string referenceId,
        decimal amount)
    {
        if (storeId == Guid.Empty)
            throw new DomainException("Invalid StoreId.");

        return Create(
            referenceId,
            PaymentType.Payout,
            amount,
            storeId: storeId);
    }

    private static PaymentTransaction Create(
        string referenceId,
        PaymentType type,
        decimal amount,
        Guid? orderId = null,
        Guid? storeId = null,
        Guid? originalPaymentId = null)
    {
        if (string.IsNullOrWhiteSpace(referenceId))
            throw new DomainException("Invalid reference ID.");

        if (amount <= 0)
            throw new DomainException("Amount must be greater than zero.");

        return new PaymentTransaction
        {
            ReferenceId = referenceId,
            Type = type,
            Status = PaymentStatus.Pending,
            Amount = amount,
            OrderId = orderId,
            StoreId = storeId,
            PaymentTransactionId = originalPaymentId
        };
    }

    public void ChangeStatus(PaymentStatus newStatus)
    {
        Status = newStatus;
    }
}