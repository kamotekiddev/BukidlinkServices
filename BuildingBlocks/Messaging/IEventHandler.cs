namespace BuildingBlocks.Messaging;

public interface IEventHandler
{
    Task HandleAsync<T>(T message) where T : IIntegrationEvent;
}