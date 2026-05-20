using NLog;
using NLog.Web;
using MessageServiceAPI.Workers;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

logger.Debug("MessageService starter op");

try
{
    var builder = Host.CreateApplicationBuilder(args);

    // Ryd eksisterende logging providers og brug NLog i stedet
    builder.Logging.ClearProviders();
    builder.Logging.AddNLog("NLog.config");

    builder.Services.AddHostedService<ClassCancelledConsumer>();

    var host = builder.Build();
    host.Run();
}
catch (Exception ex)
{
    // Logger fejl hvis applikationen crasher ved opstart
    logger.Error(ex, "MessageService stoppede på grund af en fejl!");
    throw;
}
finally
{
    // Sørg for at alle logs bliver skrevet færdigt før applikationen lukker
    NLog.LogManager.Shutdown();
}
/// <summary>
/// Worker Services bruger builder.Logging.AddNLog() i stedet for builder.Host.UseNLog()
/// fordi Worker Services ikke har en IWebHostBuilder som Web API'er har.
/// UseNLog() er en udvidelse specifikt til ASP.NET Core Web API,
/// mens AddNLog() er den generelle måde at tilføje NLog på i alle .NET applikationer.
/// </summary>
