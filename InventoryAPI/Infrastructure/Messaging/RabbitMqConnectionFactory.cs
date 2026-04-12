using Microsoft.Extensions.Options;
using ProductCatalogAPI.Infrastructure.Messaging;
using RabbitMQ.Client;

namespace InventoryAPI.Infrastructure.Messaging;

public class RabbitMqConnectionFactory(IOptions<RabbitMqOptions> options)
{
    private readonly RabbitMqOptions _options = options.Value;
    private IConnection? _connection;

    public async Task<IConnection> GetConnectionAsync()
    {
        if (_connection is not null && _connection.IsOpen)
            return _connection;

        var factory = new ConnectionFactory
        {
            HostName = _options.Host,
            UserName = _options.Username,
            Password = _options.Password,
            Port = _options.Port
        };

        _connection = await factory.CreateConnectionAsync();
        return _connection;
    }
}