namespace ProductCatalogAPI.Domain;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }

    public Product(string name, string description)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
    }
}