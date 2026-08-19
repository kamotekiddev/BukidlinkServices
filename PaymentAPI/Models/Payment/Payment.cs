using BuildingBlocks.Entities;
using BuildingBlocks.Exceptions;

namespace PaymentAPI.Models.Payment;

public class Payment : Entity
{
    private Payment()
    {
    }

    public string ReferenceId { get; init; }
    public PaymentType Type { get; init; }
    public PaymentStatus Status { get; private set; }
    public decimal Amount { get; init; }

    public static Payment Create(string referenceId, PaymentType type, PaymentStatus status, decimal amount)
    {
        if (string.IsNullOrWhiteSpace(referenceId))
            throw new DomainException("Invalid reference Id.");
        if (amount < 0) throw new DomainException("Invalid amount.");

        return new Payment
        {
            ReferenceId = referenceId,
            Type = type,
            Status = status,
            Amount = amount
        };
    }

    public void ChangeStatus(PaymentStatus newStatus)
    {
        Status = newStatus;
    }
}