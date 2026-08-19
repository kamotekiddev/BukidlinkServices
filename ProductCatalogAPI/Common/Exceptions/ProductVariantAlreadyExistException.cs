using BuildingBlocks.Exceptions;

namespace ProductCatalogAPI.Common.Exceptions;

public class ProductVariantAlreadyExistException(string title)
    : DomainException(title);