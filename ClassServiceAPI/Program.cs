using NLog;
using NLog.Web;
using ClassServiceAPI.Messaging;
using ClassServiceAPI.Repositories;
using ClassServiceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using ClassServiceAPI.Data;


var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

logger.Debug("ClassServiceAPI starter op");


try
{
    var builder = WebApplication.CreateBuilder(args);

    // Ryd eksisterende logging providers og brug NLog i stedet
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddControllers();
    builder.Services.AddOpenApi();
    builder.Services.AddScoped<IClassRepository, ClassRepository>();

    builder.Services.AddDbContext<ClassDbContext>(options =>
    {
        options.UseCosmos(
            builder.Configuration["CosmosDb:AccountEndpoint"]!,
            builder.Configuration["CosmosDb:AccountKey"]!,
            builder.Configuration["CosmosDb:DatabaseName"]!
        );
    });

    builder.Services.AddSingleton<IMessagePublisher>(sp =>
    {
        var config = sp.GetRequiredService<IConfiguration>();
        return RabbitMqPublisher.CreateAsync(config).GetAwaiter().GetResult();
    });

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
    logger.Error(ex, "ClassServiceAPI stoppede på grund af en fejl!");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}
