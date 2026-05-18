using System.Text.Json;
using RabbitMQ.Client;

namespace ClassServiceAPI.Messaging;

public class RabbitMqPublisher : IMessagePublisher, IAsyncDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;

    private RabbitMqPublisher(IConnection connection, IChannel channel)
    {
        _connection = connection;
        _channel = channel;
    }

    public static async Task<RabbitMqPublisher> CreateAsync(IConfiguration config)
    {
        var factory = new ConnectionFactory
        {
            HostName = config["RabbitMQ:Host"] ?? "localhost",
            Port     = int.Parse(config["RabbitMQ:Port"] ?? "5672"),
            UserName = config["RabbitMQ:Username"] ?? "guest",
            Password = config["RabbitMQ:Password"] ?? "guest",
        };

        var connection = await factory.CreateConnectionAsync();
        var channel    = await connection.CreateChannelAsync();

        return new RabbitMqPublisher(connection, channel);
    }

    public async Task PublishAsync<T>(T message, string queueName)
    {
        await _channel.QueueDeclareAsync(
            queue:      queueName,
            durable:    true,
            exclusive:  false,
            autoDelete: false,
            arguments:  null);

        var body = JsonSerializer.SerializeToUtf8Bytes(message);

        await _channel.BasicPublishAsync(
            exchange:   string.Empty,
            routingKey: queueName,
            body:       body);
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
    }
    public void Publish<T>(T message, string queueName)
    {
        throw new NotImplementedException();
    }
}