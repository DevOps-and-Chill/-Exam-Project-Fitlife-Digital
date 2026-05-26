using Azure.Identity;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using UserServiceAPI.Data;
using UserServiceAPI.Extensions;
using UserServiceAPI.Repositories;
using UserServiceAPI.Repositories.Interfaces;


namespace UserServiceAPI
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			// Opsæt NLog og hent en logger instans til at logge opstart og fejl
			var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

			logger.Debug("UserService starter op");

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

                //AO: Config of Cosmos for EF
                builder.Services.AddDbContext<UserDbContext>(options =>
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

                builder.Services.AddControllers()
					.AddJsonOptions(options =>
					{
						options.JsonSerializerOptions.Converters.Add(
							new System.Text.Json.Serialization.JsonStringEnumConverter());
					});

				// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
				builder.Services.AddOpenApi();

				builder.Services.AddScoped<IUserRepository, UserRepositoryDB>();
				builder.Services.AddScoped<IMemberRepository, MemberRepositoryDB>();
				builder.Services.AddScoped<IEmployeeRepository, EmployeeRepositoryDB>();

				var app = builder.Build();

				// Sikrer at DB og container eksisterer
				using (var scope = app.Services.CreateScope())
				{
					var db = scope.ServiceProvider.GetRequiredService<UserDbContext>();
					await db.Database.EnsureCreatedAsync();
				}

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
				logger.Error(ex, "UserService stoppede på grund af en fejl!");
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