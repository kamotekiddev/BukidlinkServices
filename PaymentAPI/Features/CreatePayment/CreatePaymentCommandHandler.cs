using MediatR;
using PaymentAPI.Infrastructure;
using PaymentAPI.Models.PaymentTransaction;

namespace PaymentAPI.Features.CreatePayment;

public class CreatePaymentCommandHandler(
    AppDbContext db,
    ILogger<CreatePaymentCommandHandler> logger)
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

        // handle idempotency
        // create the payment transaction record
        throw new NotImplementedException();
    }

    private string GenerateReferenceId()
    {
        var random = Guid.NewGuid().ToString("N")[..12].ToUpperInvariant();

        return $"PAY-{DateTime.UtcNow:yyyyMMdd}-{random}";
    }
}