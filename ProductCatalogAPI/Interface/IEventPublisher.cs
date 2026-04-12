using ProductCatalogAPI.Infrastructure.Messaging;

namespace ProductCatalogAPI.Interface;

public interface IEventPublisher
{
    Task PublishAsync<T>(PublisherOptions publisherOptions, T message, CancellationToken cancellationToken = default);
}