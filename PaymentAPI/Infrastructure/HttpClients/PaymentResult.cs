namespace PaymentAPI.Infrastructure.HttpClients;

public record PaymentResult(
    string ReferenceId,
    string Status,
    string? CheckoutUrl = null);