using System.Text.Json.Serialization;
using BuildingBlocks.Enums;

namespace PaymentAPI.Infrastructure.HttpClients.XenditPaymentProviderClient.Models;

public class XenditPaymentRequest
{
    [JsonPropertyName("reference_id")] public required string ReferenceId { get; init; }

    public string Type { get; init; } = "PAY";

    public string Country { get; init; } = "PH";

    public string Currency { get; init; } = "PHP";

    [JsonPropertyName("request_amount")] public decimal RequestAmount { get; init; }

    [JsonPropertyName("capture_method")] public string CaptureMethod { get; init; } = "AUTOMATIC";

    [JsonPropertyName("channel_code")] public required string ChannelCode { get; init; }

    [JsonPropertyName("channel_properties")]
    public required object ChannelProperties { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, string>? Metadata { get; init; }

    public static XenditPaymentRequest Create(
        PaymentMethod paymentMethod,
        string referenceId,
        decimal amount,
        string currency,
        string? failureReturnUrl = null,
        string? successReturnUrl = null,
        string? cancelReturnUrl = null)
    {
        return paymentMethod switch
        {
            PaymentMethod.GCash => CreateGCashPaymentRequest(
                referenceId,
                amount,
                currency,
                failureReturnUrl ?? throw new ArgumentNullException(
                    nameof(failureReturnUrl),
                    "Failure return URL is required for GCash payments."),
                successReturnUrl ?? throw new ArgumentNullException(
                    nameof(successReturnUrl),
                    "Success return URL is required for GCash payments.")),

            PaymentMethod.Maya => CreateMayaPaymentRequest(
                referenceId,
                amount,
                currency,
                failureReturnUrl ?? throw new ArgumentNullException(
                    nameof(failureReturnUrl),
                    "Failure return URL is required for Maya payments."),
                successReturnUrl ?? throw new ArgumentNullException(
                    nameof(successReturnUrl),
                    "Success return URL is required for Maya payments."),
                cancelReturnUrl ?? throw new ArgumentNullException(
                    nameof(cancelReturnUrl),
                    "Cancel return URL is required for Maya payments.")),

            _ => throw new ArgumentOutOfRangeException(
                nameof(paymentMethod),
                paymentMethod,
                "Unsupported payment method.")
        };
    }

    private static XenditPaymentRequest CreateGCashPaymentRequest(
        string referenceId,
        decimal amount,
        string currency,
        string failureReturnUrl,
        string successReturnUrl)
    {
        return new XenditPaymentRequest
        {
            ReferenceId = referenceId,
            RequestAmount = amount,
            ChannelCode = ChannelCodes.GCash,
            Currency = currency,
            ChannelProperties = new GCashChannelProperties
            {
                FailureReturnUrl = failureReturnUrl,
                SuccessReturnUrl = successReturnUrl
            }
        };
    }

    private static XenditPaymentRequest CreateMayaPaymentRequest(
        string referenceId,
        decimal amount,
        string currency,
        string failureReturnUrl,
        string successReturnUrl,
        string cancelReturnUrl)
    {
        return new XenditPaymentRequest
        {
            ReferenceId = referenceId,
            RequestAmount = amount,
            ChannelCode = ChannelCodes.Maya,
            Currency = currency,
            ChannelProperties = new MayaChannelProperties
            {
                FailureReturnUrl = failureReturnUrl,
                SuccessReturnUrl = successReturnUrl,
                CancelReturnUrl = cancelReturnUrl
            }
        };
    }
}