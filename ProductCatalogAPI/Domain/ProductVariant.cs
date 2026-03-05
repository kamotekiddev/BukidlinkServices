using ProductCatalogAPI.Domain.ValueObjects;

namespace ProductCatalogAPI.Domain;

public class ProductVariant
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Sku Sku { get; set; }
    public Money Price { get; set; }

    public Guid ProductId { get; set; }
    public Product Product { get; set; }
}