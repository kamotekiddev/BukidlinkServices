namespace ProductCatalogAPI.Domain;

public class Product
{
    private readonly List<ProductVariant> _variants = new();

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }

    public IEnumerable<ProductVariant> Variants { get; private set; }

    public Product()
    {
    }


    public Product(string name, string description)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
    }
}