using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using Scalar.AspNetCore;
using StatisticServiceAPI.Data;
using StatisticServiceAPI.Extensions;
using StatisticServiceAPI.Repositories;
using System.Text;


namespace StatisticServiceAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

            logger.Debug("StatisticService starter op");

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
                builder.Services.AddOpenApi();

                // Skiftet fra in-memory til CosmosDB repository
                builder.Services.AddScoped<IStatisticRepository, StatistikRepositoryDB>();

                //AO: Config of Cosmos for EF
                builder.Services.AddDbContext<StatisticDbContext>(options =>
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
                builder.Services
                 //AO: Tells the app that we use JWT as authentication
                 .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 //AO: Config of the token validation
                 .AddJwtBearer(options =>
                 {
                     options.TokenValidationParameters =
                     //AO: Config of what makes a token valid
                         new TokenValidationParameters
                         {
                             //AO: Check these
                             ValidateIssuer = true,
                             ValidateAudience = true,
                             ValidateLifetime = true,
                             ValidateIssuerSigningKey = true,
                             //AO: Compare issuer and audience 
                             ValidIssuer =
                                 builder.Configuration["Jwt:Issuer"],
                             ValidAudience =
                                 builder.Configuration["Jwt:Audience"],
                             //AO: Calculate key to ensure correct signature
                             IssuerSigningKey =
                                 new SymmetricSecurityKey(
                                     Encoding.UTF8.GetBytes(
                                         builder.Configuration["Jwt:Key"]!)),
                             //AO: Accept no difference in validationperiod
                             ClockSkew = TimeSpan.Zero
                         };
                 });


                var app = builder.Build();

                // Opret database og container i CosmosDB hvis de ikke findes
                using (var scope = app.Services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<StatisticDbContext>();
                    await db.Database.EnsureCreatedAsync();
                }

                if (app.Environment.IsDevelopment())
                {
                    app.MapOpenApi();
                    app.MapScalarApiReference();
                }

                app.UseHttpsRedirection();
                app.UseAuthentication();
                app.UseAuthorization();
                app.MapControllers();
                await app.RunAsync();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "StatisticService stoppede på grund af en fejl!");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }
    }
}