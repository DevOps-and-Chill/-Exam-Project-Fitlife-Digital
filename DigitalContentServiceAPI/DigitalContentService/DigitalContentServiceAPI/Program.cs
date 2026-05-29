using Azure.Identity;
using DigitalContentServiceAPI.Data;
using DigitalContentServiceAPI.Extensions;
using DigitalContentServiceAPI.Repositories;
using DigitalContentServiceAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using System.Text;

namespace DigitalContentServiceAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Opsæt NLog og hent en logger instans til at logge opstart og fejl
            var logger = NLog.LogManager
                .Setup()
                .LoadConfigurationFromAppSettings()
                .GetCurrentClassLogger();

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                await builder.LoadVault();

                //builder.Configuration.AddAzureKeyVault(
                //    new Uri("https://fitlifedigitalkv.vault.azure.net/"),
                //    new DefaultAzureCredential());

                builder.Host.UseNLog();

                // Add services to the container.
                builder.Services.AddScoped<IWorkoutProgramRepository, WorkoutProgramRepository>();
                builder.Services.AddScoped<IWorkoutVideoRepository, WorkoutVideoRepository>();

                // AO: Config of Cosmos for EF
                builder.Services.AddDbContext<DigitalContentDbContext>(options =>
                {
                    options.UseCosmos(
                        builder.Configuration["CosmosDb:AccountEndpoint"]!,
                        builder.Configuration["CosmosDb:AccountKey"]!,
                        builder.Configuration["CosmosDb:DatabaseName"]!,

                        // AO: Used during dev for CosmosDB Emulator
                        //cosmosOptions =>
                        //{
                        //    cosmosOptions.ConnectionMode(ConnectionMode.Gateway);
                        //
                        //    cosmosOptions.HttpClientFactory(() =>
                        //    {
                        //        var handler = new HttpClientHandler();
                        //
                        //        handler.ServerCertificateCustomValidationCallback =
                        //            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                        //
                        //        return new HttpClient(handler);
                        //    });
                        //});

                        cosmosOptions =>
                        {
                            cosmosOptions.ConnectionMode(ConnectionMode.Gateway);
                        });
                });

                builder.Services.AddControllers();

                // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
                builder.Services.AddOpenApi();

                builder.Services
                    // AO: Tells the app that we use JWT as authentication
                    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

                    // AO: Config of the token validation
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters =

                            // AO: Config of what makes a token valid
                            new TokenValidationParameters
                            {
                                // AO: Check these
                                ValidateIssuer = true,
                                ValidateAudience = true,
                                ValidateLifetime = true,
                                ValidateIssuerSigningKey = true,

                                // AO: Compare issuer and audience
                                ValidIssuer =
                                    builder.Configuration["Jwt:Issuer"],

                                ValidAudience =
                                    builder.Configuration["Jwt:Audience"],

                                // AO: Calculate key to ensure correct signature
                                IssuerSigningKey =
                                    new SymmetricSecurityKey(
                                        Encoding.UTF8.GetBytes(
                                            builder.Configuration["Jwt:Key"]!)),

                                // AO: Accept no difference in validationperiod
                                ClockSkew = TimeSpan.Zero
                            };
                    });

                var app = builder.Build();

                // Sikrer at DB og container eksisterer
                using (var scope = app.Services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<DigitalContentDbContext>();
                    await db.Database.EnsureCreatedAsync();
                }

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.MapOpenApi();
                }

                app.UseHttpsRedirection();

                app.UseAuthentication();
                app.UseAuthorization();

                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "DigitalContent stopped because of an unexpected error during startup");

                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }
    }
}