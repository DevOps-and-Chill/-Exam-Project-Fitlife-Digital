using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using MessageServiceAPI.Models;
using MessageServiceAPI.Services.Interfaces;

namespace MessageServiceAPI;

public class ClassCancelledConsumer : BackgroundService
{
    private readonly ILogger<ClassCancelledConsumer> _logger;
    private readonly IConfiguration _config;
    private readonly IMessageService _messageService;
    private readonly IServiceScopeFactory _scopeFactory;

    public ClassCancelledConsumer(ILogger<ClassCancelledConsumer> logger, IConfiguration config, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _config = config;
        _scopeFactory = scopeFactory;
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
            var body  = ea.Body.ToArray();
            var json  = Encoding.UTF8.GetString(body);
            var evt   = JsonSerializer.Deserialize<ClassCancelledMessage>(json);

            if (evt is not null)
                await HandleMessageAsync(evt);
        };

        await channel.BasicConsumeAsync("class.cancelled", autoAck: true, consumer);
        
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
    
    private async Task HandleMessageAsync(ClassCancelledMessage message)
    {
        using var scope = _scopeFactory.CreateScope();
        var messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
        
        foreach (var receiverId in message.ReceiverIds)
        {
            var classMessage = new ClassMessage
            {
                ReceiverId = receiverId,
                ClassId    = message.ClassId,
                Topic      = "Klasse aflyst",
                Content    = $"Din klasse '{message.Topic}' d. {message.TimeStart:dd/MM/yyyy} er aflyst.",
                TimeStart  = message.TimeStart,
                TimeEnd    = message.TimeEnd
            };

            await messageService.SendClassCancellationMessageAsync(classMessage);
        }
    }
    public override void Dispose()
    {
        base.Dispose();
    }
}