using MessageServiceAPI;
using MessageServiceAPI.Workers;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<MessageServiceAPI.Workers.ClassCancelledConsumer>();


var host = builder.Build();
host.Run();

