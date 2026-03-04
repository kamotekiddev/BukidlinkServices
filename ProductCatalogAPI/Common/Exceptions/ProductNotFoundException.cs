namespace ProductCatalogAPI.Common.Exceptions;

public class ProductNotFoundException(Guid productId)
    : Exception($"Product with the given Id {productId} not found.")
{
}