using System.Text.Json.Serialization;

namespace PaymentAPI.Infrastructure.HttpClients.XenditPaymentProviderClient.Models;

public record XenditErrorResponse(
    [property: JsonPropertyName("error_code")]
    string ErrorCode,
    string Message);