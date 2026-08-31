using System.Text.Json;
using System.Text.Json.Serialization;
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

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };

        using var response = await client.PostAsJsonAsync(
            "/v3/payment_requests",
            request,
            jsonOptions,
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


        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        var result = JsonSerializer.Deserialize<XenditPaymentResponse>(responseBody, jsonOptions);

        if (result is null)
        {
            logger.LogWarning(
                "Payment failed: {Provider} returned an invalid response. ResponseBody:{ResponseBody}",
                nameof(XenditPaymentProviderClient),
                responseBody);

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