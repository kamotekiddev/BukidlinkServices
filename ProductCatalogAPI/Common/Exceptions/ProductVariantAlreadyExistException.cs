namespace ProductCatalogAPI.Common.Exceptions;

public class ProductVariantAlreadyExistException(string message)
    : DomainException(message);