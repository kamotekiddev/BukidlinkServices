using MediatR;
using OrderAPI.Models;

namespace OrderAPI.Features.Orders.CreateOrder;

public class CreateOrderCommandHandler:IRequestHandler<CreateOrderCommand, Order>
{
    public Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}