using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using PTServiceAPI.Data;
using PTServiceAPI.Repositories;
using Scalar.AspNetCore;
using System.Threading.Tasks;

namespace PTServiceAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

            logger.Debug("PTService starter op");

            try
            {
                //JBS: Opsæt NLog og hent en logger instans til at logge opstart og fejl 
                var builder = WebApplication.CreateBuilder(args);

                //JBS: Ryd eksisterende logging providers og så bruger vi NLog i stedet for.
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();

                //JBS: Services til controller
                builder.Services.AddControllers();
                // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
                builder.Services.AddOpenApi();
                builder.Services.AddScoped<ISessionRepository, SessionRepositoryDB>();

                //JBS: Tilføj cosmos via EF
                builder.Services.AddDbContext<PTDbContext>(options =>
                {
                    options.UseCosmos(
                        builder.Configuration["CosmosDb:AccountEndpoint"]!,
                        builder.Configuration["CosmosDb:AccountKey"]!,
                        builder.Configuration["CosmosDb:DatabaseName"]!,
                        cosmosOptions =>
                        {
                            cosmosOptions.ConnectionMode(ConnectionMode.Gateway);

                            cosmosOptions.HttpClientFactory(() =>
                            {
                                var handler = new HttpClientHandler();

                                handler.ServerCertificateCustomValidationCallback =
                                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

                                return new HttpClient(handler);
                            });
                        });
                });

                var app = builder.Build();

                // Opret database og container i CosmosDB hvis de ikke findes
                using (var scope = app.Services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<PTDbContext>();
                    await db.Database.EnsureCreatedAsync();
                }

                //JBS: Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.MapOpenApi();
                    app.MapScalarApiReference(); //Scalar UI til API dokumentation
                }

                app.UseHttpsRedirection();
                app.UseAuthorization();
                app.MapControllers();
                await app.RunAsync();
            }
            catch (Exception ex)
            {
                //JBS: Vi logger fejl hvis applikationen crasher ved opstarten
                logger.Error(ex, "PTService stoppede på grund af en fejl!");
                throw;
            }
            finally
            {
                //JBS: Vi skal sørge for at alle logs bliver skrevet færdigt før at applikationen lukker ned
                NLog.LogManager.Shutdown();
            }
        }
    }
}
