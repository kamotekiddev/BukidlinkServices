namespace PaymentAPI.Infrastructure.HttpClients.XenditPaymentProviderClient.Models;

public class GCashChannelProperties : ChannelProperties
{
    public string FailureReturnUrl { get; init; }
    public string SuccessReturnUrl { get; init; }
}