using Azure.Identity;
using DigitalContentServiceAPI.Data;
using DigitalContentServiceAPI.Extensions;
using DigitalContentServiceAPI.Repositories;
using DigitalContentServiceAPI.Repositories.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;

namespace DigitalContentServiceAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            await builder.LoadVault();
            //builder.Configuration.AddAzureKeyVault(
            //        new Uri("https://fitlifedigitalkv.vault.azure.net/"),
            //        new DefaultAzureCredential());


            // Add services to the container.
            builder.Services.AddScoped<IWorkoutProgramRepository, WorkoutProgramRepository>();
            builder.Services.AddScoped<IWorkoutVideoRepository, WorkoutVideoRepository>();

            //AO: Config of Cosmos for EF
            builder.Services.AddDbContext<DigitalContentDbContext>(options =>
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


            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
