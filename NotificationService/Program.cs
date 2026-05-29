using NLog;
using NLog.Web;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

logger.Debug("NotificationService starter op");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Ryd eksisterende logging providers og brug NLog i stedet
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddControllers();
    builder.Services.AddOpenApi();

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
    logger.Error(ex, "NotificationService stoppede på grund af en fejl!");
    throw;
}
finally
{
    // Sørg for at alle logs bliver skrevet færdigt før applikationen lukker
    NLog.LogManager.Shutdown();
}