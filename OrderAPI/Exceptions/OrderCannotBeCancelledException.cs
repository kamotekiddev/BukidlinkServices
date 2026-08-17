using BuildingBlocks.Exceptions;
using OrderAPI.Models.Enums;

namespace OrderAPI.Exceptions;

public class OrderCannotBeCancelledException : DomainException
{
    public OrderCannotBeCancelledException(OrderStatus status)
        : base($"An order with status '{status}' cannot be cancelled.")
    {
    }
}