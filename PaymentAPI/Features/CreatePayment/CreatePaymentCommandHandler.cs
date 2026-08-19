using MediatR;

namespace PaymentAPI.Features.CreatePayment;

public class CreatePaymentCommandHandler(ILogger<CreatePaymentCommandHandler> logger)
    : IRequestHandler<CreatePaymentCommand, CreatePaymentResult>
{
    public Task<CreatePaymentResult> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}