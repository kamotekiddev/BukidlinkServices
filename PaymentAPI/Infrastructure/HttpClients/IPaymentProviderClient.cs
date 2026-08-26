namespace PaymentAPI.Infrastructure.HttpClients;

public interface IPaymentProviderClient
{
    Task<PaymentResult> PayAsync(
        PaymentRequest payment,
        CancellationToken cancellationToken = default);
}