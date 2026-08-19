using BuildingBlocks.Exceptions;

namespace ProductCatalogAPI.Common.Exceptions;

public class InvalidSkuException(string title) : DomainException(title);