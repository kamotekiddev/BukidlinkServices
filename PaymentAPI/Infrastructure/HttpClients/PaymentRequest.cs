using BuildingBlocks.Enums;

namespace PaymentAPI.Infrastructure.HttpClients;

public sealed class PaymentRequest
{
    public required string ReferenceId { get; init; }
    public required decimal Amount { get; init; }
    public required PaymentMethod Method { get; init; }
    public string Currency { get; init; } = "PHP";

    public PaymentCustomer? Customer { get; init; }
    public CardDetails? Card { get; init; }
    public RedirectUrls? RedirectUrls { get; init; }
}

public abstract class PaymentCustomer
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public string? PhoneNumber { get; init; }
}

public abstract class CardDetails
{
    public required string CardNumber { get; init; }
    public required string Cvc { get; init; }
    public required string ExpiryMonth { get; init; }
    public required string ExpiryYear { get; init; }
}

public abstract class RedirectUrls
{
    public string? FailureReturnUrl { get; init; }
    public string? SuccessReturnUrl { get; init; }
    public string? CancelReturnUrl { get; init; }
}