namespace BuildingBlocks.Messaging;

public interface IMessagePublisher
{
    Task PublishAsync<T>(T message) where T : IIntegrationEvent;
}