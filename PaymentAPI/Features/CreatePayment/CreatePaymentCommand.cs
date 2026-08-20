using MediatR;

namespace PaymentAPI.Features.CreatePayment;

public record CreatePaymentCommand(Guid OrderId, decimal Amount) : IRequest<CreatePaymentResult>;