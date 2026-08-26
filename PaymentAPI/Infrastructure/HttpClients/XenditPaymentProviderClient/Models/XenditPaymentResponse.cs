using System.Text.Json.Serialization;

namespace PaymentAPI.Infrastructure.HttpClients.XenditPaymentProviderClient.Models;

public sealed class XenditPaymentResponse
{
    [JsonPropertyName("business_id")] public required string BusinessId { get; init; }

    [JsonPropertyName("reference_id")] public required string ReferenceId { get; init; }

    [JsonPropertyName("payment_request_id")]
    public required string PaymentRequestId { get; init; }

    [JsonPropertyName("request_amount")] public decimal RequestAmount { get; init; }

    [JsonPropertyName("capture_method")] public required string CaptureMethod { get; init; }

    [JsonPropertyName("channel_code")] public required string ChannelCode { get; init; }

    [JsonPropertyName("channel_properties")]
    public object? ChannelProperties { get; init; }

    public List<PaymentAction> Actions { get; init; } = [];

    public required string Status { get; init; }

    public string? Description { get; init; }

    public Dictionary<string, string>? Metadata { get; init; }

    public DateTime Created { get; init; }

    public DateTime Updated { get; init; }
}

public static class PaymentStatuses
{
    public const string AcceptingPayments = "ACCEPTING_PAYMENTS";
    public const string RequiresAction = "REQUIRES_ACTION";
    public const string Authorized = "AUTHORIZED";
    public const string Canceled = "CANCELED";
    public const string Expired = "EXPIRED";
    public const string Succeeded = "SUCCEEDED";
    public const string Failed = "FAILED";
}

public sealed class PaymentAction
{
    public required PaymentActionType Type { get; init; }
    public required PaymentActionDescriptor Descriptor { get; init; }
    public required string Value { get; init; }
}

public enum PaymentActionType
{
    PRESENT_TO_CUSTOMER,
    REDIRECT_CUSTOMER,
    API_POST_REQUEST
}

public enum PaymentActionDescriptor
{
    CAPTURE_PAYMENT,
    PAYMENT_CODE,
    QR_STRING,
    VIRTUAL_ACCOUNT_NUMBER,
    WEB_URL,
    DEEPLINK_URL,
    VALIDATE_OTP,
    RESEND_OTP
}