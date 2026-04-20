namespace InventoryAPI.Common.Exceptions;

public class ProductVariantAlreadyExistException(Guid productVariantId)
    : Exception($"Product Variant with id {productVariantId} already exist.");