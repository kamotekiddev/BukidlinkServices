using BuildingBlocks.Entities;

namespace ProductCatalogAPI.Domain;

public class Product : Entity
{
    private Product()
    {
    }

    public string Name { get; private set; }
    public string Description { get; private set; }

    public Guid StoreId { get; init; }
    public IEnumerable<ProductVariant> Variants { get; init; }


    public static Product Create(string name, string description, Guid storeId)
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            StoreId = storeId
        };
    }
}