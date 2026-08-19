namespace OrderAPI.Infrastructure.HttpClients.ProductServiceClient.Models;

public record GetProductVariantByIdsRequest(ICollection<Guid> VariantIds);