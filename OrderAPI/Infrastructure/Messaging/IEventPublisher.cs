namespace OrderAPI.Infrastructure.Messaging;

public interface IEventPublisher
{
    Task PublishAsync<T>(PublisherOptions publisherOptions, T message, CancellationToken cancellationToken = default);
}