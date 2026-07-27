using BuildingBlocks.Entities;
using ProductCatalogAPI.Domain.ValueObjects;

namespace ProductCatalogAPI.Domain;

public class ProductVariant : Entity
{
    public string Name { get; private set; }
    public Sku Sku { get; private set; }
    public Money Price { get; private set; }

    public Guid ProductId { get; init; }
    public Product Product { get; init; }

    public static ProductVariant Create(string name, Sku sku, Money price, Guid productId)
    {
        return new ProductVariant
        {
            Name = name,
            Sku = sku,
            Price = price,
            ProductId = productId
        };
    }
}