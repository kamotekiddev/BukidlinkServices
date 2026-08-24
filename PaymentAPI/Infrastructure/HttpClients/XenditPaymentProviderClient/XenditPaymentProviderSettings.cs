namespace PaymentAPI.Infrastructure.HttpClients.XenditPaymentProviderClient;

public class XenditPaymentProviderSettings
{
    public string BaseUrl { get; set; } = "https://api.xendit.co";
    public string ApiVersion { get; set; } = "2024-11-11";
    public string SecretKey { get; set; }
}