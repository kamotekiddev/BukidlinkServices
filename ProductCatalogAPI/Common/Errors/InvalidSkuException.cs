using ProductCatalogAPI.Common.Exceptions;

namespace ProductCatalogAPI.Common.Errors;

public class InvalidSkuException(string message) : DomainException(message);