using System.Text.Json.Serialization;

namespace PaymentAPI.Infrastructure.HttpClients.XenditPaymentProviderClient.Models;

public class GCashChannelProperties : ChannelProperties
{
    [JsonPropertyName("failure_return_url")]
    public string FailureReturnUrl { get; init; }

    [JsonPropertyName("success_return_url")]
    public string SuccessReturnUrl { get; init; }
}