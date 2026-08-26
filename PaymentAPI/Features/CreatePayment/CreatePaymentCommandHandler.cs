using MediatR;
using PaymentAPI.Infrastructure;
using PaymentAPI.Infrastructure.HttpClients;
using PaymentAPI.Models.PaymentTransaction;

namespace PaymentAPI.Features.CreatePayment;

public class CreatePaymentCommandHandler(
    AppDbContext db,
    ILogger<CreatePaymentCommandHandler> logger,
    IPaymentProviderClient paymentProviderClient)
    : IRequestHandler<CreatePaymentCommand, CreatePaymentResult>
{
    public async Task<CreatePaymentResult> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var referenceId = GenerateReferenceId();

        var payment = PaymentTransaction.CreatePayment(
            request.OrderId,
            referenceId,
            request.Amount
        );

        db.Payments.Add(payment);
        await db.SaveChangesAsync(cancellationToken);


        // trigger provider payment
        var paymentRequest = new PaymentRequest
        {
            ReferenceId = referenceId,
            Amount = request.Amount,
            Method = request.PaymentMethod,
            RedirectUrls = request.RedirectUrls
        };

        try
        {
            var paymentResult = await paymentProviderClient.PayAsync(paymentRequest, cancellationToken);
            return new CreatePaymentResult(paymentResult?.CheckoutUrl ?? "");
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Payment failed for OrderId:{OrderId} ReferenceId:{ReferenceId}",
                request.OrderId,
                referenceId);
            throw;
        }
    }

    private string GenerateReferenceId()
    {
        var random = Guid.NewGuid().ToString("N")[..12].ToUpperInvariant();

        return $"PAY-{DateTime.UtcNow:yyyyMMdd}-{random}";
    }
}