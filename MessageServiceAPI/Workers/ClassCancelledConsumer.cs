using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using MessageServiceAPI.Messaging;

namespace MessageServiceAPI.Workers;

public class ClassCancelledConsumer : BackgroundService
{
    private readonly ILogger<ClassCancelledConsumer> _logger;
    private readonly IConfiguration _config;

    private IConnection? _connection;

    public ClassCancelledConsumer(ILogger<ClassCancelledConsumer> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _config["RabbitMQ:Host"] ?? "localhost"
        };

        var connection = await factory.CreateConnectionAsync();
        var channel    = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue:      "class.cancelled",
            durable:    true,
            exclusive:  false,
            autoDelete: false,
            arguments:  null);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            var body    = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var obj     = JsonSerializer.Deserialize<ClassCancelledMessage>(message);

            if (obj is not null)
                _logger.LogInformation("Klasse '{Title}' aflyst for {Count} members",
                    obj.Title, obj.MemberIds.Count);

            return Task.CompletedTask;
        };

        await channel.BasicConsumeAsync("class.cancelled", autoAck: true, consumer);
        
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private Task HandleMessageAsync(ClassCancelledMessage message)
    {
        _logger.LogInformation(
            "Klasse '{Title}' (ID: {ClassId}) er aflyst. " +
            "Notificerer {Count} members: {MemberIds}",
            message.Title,
            message.ClassId,
            message.MemberIds.Count,
            string.Join(", ", message.MemberIds));

        /* TODO: kald service
        foreach (var memberId in message.MemberIds)
        await _Service.SendCancellationEmailAsync(memberId, message);
        */
        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}