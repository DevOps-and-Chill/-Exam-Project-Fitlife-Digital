using MessageServiceAPI.Data;
using MessageServiceAPI.Services;
using MessageServiceAPI.Services.Interfaces;
using MessageServiceAPI.Workers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<MessageDbContext>(options =>
    options.UseInMemoryDatabase("MessageDb"));
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddHostedService<ClassCancelledConsumer>();

var app = builder.Build();

app.MapControllers();
app.Run();