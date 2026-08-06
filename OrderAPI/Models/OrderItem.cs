namespace OrderAPI.Models;

public class OrderItem
{
    private OrderItem()
    {
    }

    public Guid Id { get; init; }
    public Guid OrderId { get; init; }
    public Guid ProductVariantId { get; init; }
    public int Quantity { get; init; }
    public decimal SellPrice { get; init; }

    public static OrderItem Create(Guid productVariantId, int quantity, decimal sellPrice)
    {
        return new OrderItem
        {
            ProductVariantId = productVariantId,
            Quantity = quantity,
            SellPrice = sellPrice
        };
    }
}