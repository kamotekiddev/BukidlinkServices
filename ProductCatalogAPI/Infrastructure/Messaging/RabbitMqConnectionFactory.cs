using Microsoft.Extensions.Options;
using ProductCatalogAPI.Infrastructure.Messaging;
using RabbitMQ.Client;

public class RabbitMqConnectionFactory
{
    private readonly RabbitMqOptions _options;
    private IConnection? _connection;

    public RabbitMqConnectionFactory(IOptions<RabbitMqOptions> options)
    {
        _options = options.Value;
    }

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