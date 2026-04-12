using ProductCatalogAPI.Interface;

public class RabbitMqEventBus : IEventBus
{
    private readonly RabbitMqPublisher _publisher;

    public RabbitMqEventBus(RabbitMqPublisher publisher)
    {
        _publisher = publisher;
    }

    public Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default)
    {
        var eventName = typeof(T).Name;

        return _publisher.PublishAsync(
            exchange: "product.events",
            routingKey: eventName,
            message: @event);
    }
}