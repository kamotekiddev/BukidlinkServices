namespace ProductCatalogAPI.Domain.ValueObjects;

public record Money(decimal Value, string Currency = "PHP");