using NLog;
using NLog.Web;
using RapportServiceAPI.Repositories;
using Scalar.AspNetCore;

namespace RapportServiceAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Opsæt NLog og hent en logger instans til at logge opstart og fejl
            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

            logger.Debug("RapportService starter op");

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                // Ryd eksisterende logging providers og brug NLog i stedet
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();

                builder.Services.AddControllers();
                builder.Services.AddOpenApi();
                builder.Services.AddScoped<IRapportRepository, RapportRepository>();

                var app = builder.Build();

                if (app.Environment.IsDevelopment())
                {
                    app.MapOpenApi();
                    app.MapScalarApiReference();
                }

                app.UseHttpsRedirection();
                app.UseAuthorization();
                app.MapControllers();
                app.Run();
            }
            catch (Exception ex)
            {
                // Logger fejl hvis applikationen crasher ved opstart
                logger.Error(ex, "RapportService stoppede på grund af en fejl!");
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