namespace PaymentAPI.Infrastructure.HttpClients.XenditPaymentProviderClient.Models;

public class MayaChannelProperties : ChannelProperties
{
    public string? FailureReturnUrl { get; init; }
    public string? SuccessReturnUrl { get; init; }
    public string? CancelReturnUrl { get; init; }
}