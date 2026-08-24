using PaymentAPI.Models.PaymentTransaction;

namespace PaymentAPI.Infrastructure.HttpClients;

public record PaymentResult(
    string ProviderReferenceId,
    PaymentStatus Status,
    string? CheckoutUrl = null);