namespace OrderAPI.Models;

public class OrderItem
{
    public Guid Id { get; init; }
    public Guid OrderId { get; init; }
    public Guid ProductVariantId { get; init; }
    public int Quantity { get; init; }
    public decimal SellPrice { get; init; }
}