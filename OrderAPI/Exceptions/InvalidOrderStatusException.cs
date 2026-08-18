using BuildingBlocks.Exceptions;
using OrderAPI.Models.Enums;

namespace OrderAPI.Exceptions;

public class InvalidOrderStatusException : DomainException
{
    public InvalidOrderStatusException(OrderStatus status) : base($"Invalid Order status. Status:{status}")
    {
    }
}