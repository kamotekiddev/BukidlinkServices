namespace ProductCatalogAPI.Common.Exceptions;

public class ProductAlreadyExistsException(string productName)
    : DomainException($"A product with the name '{productName}' already exists.");