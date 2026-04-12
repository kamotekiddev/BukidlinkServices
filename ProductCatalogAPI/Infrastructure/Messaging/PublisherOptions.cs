namespace ProductCatalogAPI.Infrastructure.Messaging;

public class PublisherOptions
{
    public required string Exchange { get; init; }
    public required string RoutingKey { get; init; }
}