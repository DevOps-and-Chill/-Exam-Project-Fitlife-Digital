using Azure.Identity;
using FacilityServiceAPI.Contexts;
using FacilityServiceAPI.Extensions;
using FacilityServiceAPI.Repositories;
using FacilityServiceAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using System.Configuration;
using System.Text;

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

                await builder.LoadVault();
                //builder.Configuration.AddAzureKeyVault(
                //        new Uri("https://fitlifedigitalkv.vault.azure.net/"),
                //        new DefaultAzureCredential());


                // Ryd eksisterende logging providers og brug NLog i stedet
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();

			builder.Services.AddControllers();
			// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
			builder.Services.AddOpenApi();

			builder.Services.AddTransient<IFacilityRepository, FacilityRepository>();

                //Enables dependency injection of Factory pattern for DBContext. This way the application is more threadsafe, because each 
            builder.Services.AddDbContextFactory<FacilityContext>(options =>
            {
                options.UseCosmos(
                        builder.Configuration["CosmosDb:AccountEndpoint"]!,
                        builder.Configuration["CosmosDb:AccountKey"]!,
                        builder.Configuration["CosmosDb:DatabaseName"]!,
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

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.MapOpenApi();
			}
			using (var scope = app.Services.CreateScope())
			{
				var db = scope.ServiceProvider.GetRequiredService<FacilityContext>();

				await db.Database.EnsureCreatedAsync();
			}

                app.UseHttpsRedirection();
                app.UseAuthentication();
                app.UseAuthorization();
                app.MapControllers();
                app.Run();
            }
            catch (Exception ex)
            {
                // Logger fejl hvis applikationen crasher ved opstart
                logger.Error(ex, "FacilityService stoppede på grund af en fejl!");
                throw;
            }
            finally
            {
                // Sørg for at alle logs bliver skrevet færdigt før applikationen lukker
                NLog.LogManager.Shutdown();
            }
        }
    }
}