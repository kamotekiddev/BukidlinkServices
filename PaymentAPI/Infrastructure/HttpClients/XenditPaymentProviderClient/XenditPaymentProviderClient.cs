using PaymentAPI.Infrastructure.HttpClients.XenditPaymentProviderClient.Models;

namespace PaymentAPI.Infrastructure.HttpClients.XenditPaymentProviderClient;

public class XenditPaymentProviderClient(
    ILogger<XenditPaymentProviderClient> logger,
    HttpClient client)
    : IPaymentProviderClient
{
    public async Task<PaymentResult> PayAsync(
        PaymentRequest payment,
        CancellationToken cancellationToken)
    {
        var requerst = XenditPaymentRequest.Create(
            payment.Method,
            payment.ReferenceId,
            payment.Amount,
            payment.Currency,
            payment.RedirectUrls.FailureReturnUrl,
            payment.RedirectUrls.SuccessReturnUrl,
            payment.RedirectUrls.CancelReturnUrl);
        var response = await client.PostAsJsonAsync("/v3/payment-requests", new { }, cancellationToken);


        throw new NotImplementedException();
    }
}