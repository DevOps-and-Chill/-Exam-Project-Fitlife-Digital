using Azure.Identity;
using MessageServiceAPI.Extensions;
using MessageServiceAPI;
using MessageServiceAPI.Data;
using MessageServiceAPI.Services;
using MessageServiceAPI.Services.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("MessageService starter op");

try
{
    var builder = WebApplication.CreateBuilder(args);

    await builder.LoadVault();
    //builder.Configuration.AddAzureKeyVault(
    //        new Uri("https://fitlifedigitalkv.vault.azure.net/"),
    //        new DefaultAzureCredential());

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddControllers();

    //AO: Config of Cosmos for EF
    builder.Services.AddDbContext<MessageDbContext>(options =>
    {
        options.UseCosmos(
            builder.Configuration["CosmosDb:AccountEndpoint"]!,
            builder.Configuration["CosmosDb:AccountKey"]!,
            builder.Configuration["CosmosDb:DatabaseName"]!,
               //AO: Used during dev for CosmosDB Emulator 
               //cosmosOptions =>
               //{
               //    cosmosOptions.ConnectionMode(ConnectionMode.Gateway);

               //    cosmosOptions.HttpClientFactory(() =>
               //    {
               //        var handler = new HttpClientHandler();

               //        handler.ServerCertificateCustomValidationCallback =
               //            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

               //        return new HttpClient(handler);
               //    });
               //});
               cosmosOptions =>
               {
                   cosmosOptions.ConnectionMode(ConnectionMode.Gateway);
               });
    });


    builder.Services.AddScoped<IMessageService, MessageService>();
    
    // Så ClassCancelledConsumer kun starter uden for development. Forhindrer run errors.
    if (!builder.Environment.IsDevelopment())
    {
        builder.Services.AddHostedService<ClassCancelledConsumer>();
    }
    

    var app = builder.Build();

    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "MessageService stoppede på grund af en fejl!");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}