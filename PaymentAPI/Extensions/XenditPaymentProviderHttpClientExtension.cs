using System.Net.Http.Headers;
using System.Text;
using PaymentAPI.Infrastructure.HttpClients;
using PaymentAPI.Infrastructure.HttpClients.XenditPaymentProviderClient;

namespace PaymentAPI.Extensions;

public static class XenditPaymentProviderHttpClientExtension
{
    public static IServiceCollection AddXenditHttpClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = configuration
                           .GetSection(nameof(XenditPaymentProviderSettings))
                           .Get<XenditPaymentProviderSettings>() ??
                       throw new InvalidOperationException($"{nameof(XenditPaymentProviderSettings)} is missing.");

        services.AddHttpClient<IPaymentProviderClient, XenditPaymentProviderClient>(client =>
        {
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{settings.SecretKey}:"));

            client.BaseAddress = new Uri(settings.BaseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
            client.DefaultRequestHeaders.Add("api-version", settings.ApiVersion);
        });

        return services;
    }
}