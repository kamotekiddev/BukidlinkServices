using BuildingBlocks.Enums;
using MediatR;
using PaymentAPI.Infrastructure.HttpClients;

namespace PaymentAPI.Features.CreatePayment;

public record CreatePaymentCommand(
    Guid OrderId,
    decimal Amount,
    PaymentMethod PaymentMethod,
    RedirectUrls? RedirectUrls)
    : IRequest<CreatePaymentResult>;