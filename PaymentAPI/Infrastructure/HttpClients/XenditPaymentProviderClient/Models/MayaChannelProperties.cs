using System.Text.Json.Serialization;

namespace PaymentAPI.Infrastructure.HttpClients.XenditPaymentProviderClient.Models;

public class MayaChannelProperties
{
    [JsonPropertyName("failure_return_url")]
    public string? FailureReturnUrl { get; init; }

    [JsonPropertyName("success_return_url")]
    public string? SuccessReturnUrl { get; init; }

    [JsonPropertyName("cancel_return_url")]
    public string? CancelReturnUrl { get; init; }
}