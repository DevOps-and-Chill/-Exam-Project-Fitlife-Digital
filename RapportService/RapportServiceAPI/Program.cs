using Microsoft.Azure.Cosmos;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using RapportServiceAPI.Data;
using RapportServiceAPI.Repositories;
using Scalar.AspNetCore;

namespace RapportServiceAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

            logger.Debug("RapportService starter op");

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                //AO: Config for KeyVault
                builder.Configuration.AddAzureKeyVault(
                    new Uri("https://fitlifedigitalkv.vault.azure.net/"),
                    new DefaultAzureCredential());


                builder.Logging.ClearProviders();
                builder.Host.UseNLog();

                builder.Services.AddControllers();
                builder.Services.AddOpenApi();

                // Skiftet fra in-memory til CosmosDB repository
                builder.Services.AddScoped<IRapportRepository, StatistikRepositoryDB>();

                //AO: Config of Cosmos for EF
                builder.Services.AddDbContext<RapportDbContext>(options =>
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


                var app = builder.Build();

                // Opret database og container i CosmosDB hvis de ikke findes
                using (var scope = app.Services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<RapportDbContext>();
                    await db.Database.EnsureCreatedAsync();
                }

                if (app.Environment.IsDevelopment())
                {
                    app.MapOpenApi();
                    app.MapScalarApiReference();
                }

                app.UseHttpsRedirection();
                app.UseAuthorization();
                app.MapControllers();
                await app.RunAsync();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "RapportService stoppede på grund af en fejl!");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }
    }
}