using NLog;
using NLog.Web;
using ClassServiceAPI.Messaging;
using ClassServiceAPI.Repositories;
using ClassServiceAPI.Repositories.Interfaces;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

logger.Debug("ClassServiceAPI starter op");

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IClassRepository, ClassRepository>();

builder.Services.AddSingleton<IMessagePublisher>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return RabbitMqPublisher.CreateAsync(config).GetAwaiter().GetResult();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var builder = WebApplication.CreateBuilder(args);

    // Ryd eksisterende logging providers og brug NLog i stedet
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddControllers();
    builder.Services.AddOpenApi();
    builder.Services.AddScoped<IClassRepository, ClassRepository>();
    builder.Services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    // Logger fejl hvis applikationen crasher ved opstart
    logger.Error(ex, "ClassServiceAPI stoppede på grund af en fejl!");
    throw;
}
finally
{
    // Sørg for at alle logs bliver skrevet færdigt før applikationen lukker
    NLog.LogManager.Shutdown();
}