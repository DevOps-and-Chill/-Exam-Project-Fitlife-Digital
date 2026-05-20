using System.Net.Http.Json;
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
    private readonly HttpClient _httpClient;

    private IConnection? _connection;

    public ClassCancelledConsumer(ILogger<ClassCancelledConsumer> logger, IConfiguration config, HttpClient httpClient)
    {
        _logger = logger;
        _config = config;
        _httpClient = httpClient;
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
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body    = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var obj     = JsonSerializer.Deserialize<ClassCancelledMessage>(message);

            if (obj is not null)
                await HandleMessageAsync(obj);
        };

        await channel.BasicConsumeAsync("class.cancelled", autoAck: true, consumer);
        
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task HandleMessageAsync(ClassCancelledMessage message)
    {
        _logger.LogInformation(
            "Klasse '{Title}' (ID: {ClassId}) er aflyst. " +
            "Notificerer {Count} members: {MemberIds}",
            message.Title,
            message.ClassId,
            message.MemberIds.Count,
            string.Join(", ", message.MemberIds));

        foreach (var memberId in message.MemberIds)
        {
            await _httpClient.PostAsJsonAsync("http://inboxservice/api/inbox", new
            {
                MemberId = memberId,
                Title = "Klasse aflyst",
                Body = $"Din klasse '{message.Title}' d. {message.TimeStart:dd/MM/yyyy} er aflyst."
            });
        }
    }


    public override void Dispose()
    {
        base.Dispose();
    }
}