namespace PaymentAPI.Infrastructure.HttpClients.XenditPaymentProviderClient.Models;

public class CardDetails
{
    public required string Cvn { get; init; }
    public required string CardNumber { get; init; }
    public required string ExpiryYear { get; init; }
    public required string ExpiryMonth { get; init; }
    public required string CardholderFirstName { get; init; }
    public required string CardholderLastName { get; init; }
    public required string CardholderEmail { get; init; }
    public required string CardholderPhoneNumber { get; init; }
}