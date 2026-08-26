namespace PaymentAPI.Infrastructure.HttpClients.XenditPaymentProviderClient;

public class XenditPaymentProviderSettings
{
    public string BaseUrl { get; set; }
    public string ApiVersion { get; set; }
    public string SecretKey { get; set; }
}