using NLog;
using NLog.Web;
using Azure.Identity;
using FacilityServiceAPI.Contexts;
using FacilityServiceAPI.Repositories;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using FacilityServiceAPI.Repositories.Interfaces;

namespace FacilityServiceAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Opsæt NLog og hent en logger instans til at logge opstart og fejl
            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

            logger.Debug("FacilityService starter op");

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                //builder.Configuration.AddAzureKeyVault(
                //        new Uri("https://fitlifedigitalkv.vault.azure.net/"),
                //        new DefaultAzureCredential());


                // Ryd eksisterende logging providers og brug NLog i stedet
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();

			builder.Services.AddControllers();
			// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
			builder.Services.AddOpenApi();

			builder.Services.AddTransient<IFacilityRepository, FacilityRepositoryMoq>();
            builder.Services.AddTransient<IExerciseGymRepository, FacilityRepositoryMoq>();
				builder.Services.AddTransient<ISwimmingPoolRepository, FacilityRepositoryMoq>();


				//Enables dependency injection of Factory pattern for DBContext. This way the application is more threadsafe, because each context is managed through the factory
				//    builder.Services.AddDbContextFactory<FacilityContext>(options =>
				//    {
				//        options.UseCosmos(
				//            builder.Configuration["CosmosDb:AccountEndpoint"]!,
				//            builder.Configuration["CosmosDb:AccountKey"]!,
				//            builder.Configuration["CosmosDb:DatabaseName"]!,
				//            //cosmosOptions =>
				//            //{
				//            //    cosmosOptions.ConnectionMode(ConnectionMode.Gateway);

				//            //    cosmosOptions.HttpClientFactory(() =>
				//            //    {
				//            //        var handler = new HttpClientHandler();

				//            //        handler.ServerCertificateCustomValidationCallback =
				//            //            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

				//            //        return new HttpClient(handler);
				//            //    });
				//            //});
				//            cosmosOptions =>
				//            {
				//                cosmosOptions.ConnectionMode(ConnectionMode.Gateway);
				//            });
				//});

				var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.MapOpenApi();
			}
			// using (var scope = app.Services.CreateScope())
			// {
			// 	var db = scope.ServiceProvider.GetRequiredService<FacilityContext>();

			// 	await db.Database.EnsureCreatedAsync();
			// }

                app.UseHttpsRedirection();
                //app.UseAuthorization();
                app.MapControllers();
                app.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "FacilityService stopped because of an unexpected error durring startup");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }
    }
}