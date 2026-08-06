namespace OrderAPI.Infrastructure.HttpClients.ProductServiceClient.Models;

public record GetProductVariantByIdsRequest(IEnumerable<Guid> VariantIds);