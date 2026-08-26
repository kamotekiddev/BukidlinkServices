using System.Text.Json;
using BuildingBlocks.Exceptions;
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
        var request = XenditPaymentRequest.Create(
            payment.Method,
            payment.ReferenceId,
            payment.Amount,
            payment.Currency,
            payment.RedirectUrls?.FailureReturnUrl,
            payment.RedirectUrls?.SuccessReturnUrl,
            payment.RedirectUrls?.CancelReturnUrl);

        var body = JsonSerializer.Serialize(request);

        using var response = await client.PostAsJsonAsync(
            "/v3/payment_requests",
            request,
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content
                .ReadFromJsonAsync<XenditErrorResponse>(cancellationToken);

            logger.LogWarning(
                "Payment failed: {Provider}. Message: {Message}",
                nameof(XenditPaymentProviderClient),
                error?.Message);

            throw new ConflictException(
                error?.Message ?? "Payment failed.");
        }

        var result = await response.Content
            .ReadFromJsonAsync<XenditPaymentResponse>(cancellationToken);

        if (result is null)
        {
            logger.LogWarning(
                "Payment failed: {Provider} returned an invalid response.",
                nameof(XenditPaymentProviderClient));

            throw new ConflictException("Payment failed.");
        }

        var checkoutUrl = result.Actions
            .FirstOrDefault(a =>
                a.Type == PaymentActionType.REDIRECT_CUSTOMER &&
                a.Descriptor == PaymentActionDescriptor.WEB_URL)
            ?.Value;

        return new PaymentResult(
            result.ReferenceId,
            result.Status,
            checkoutUrl);
    }
}