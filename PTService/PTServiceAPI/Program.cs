using NLog;
using NLog.Web;
using PTServiceAPI.Repositories;
using Scalar.AspNetCore;

namespace PTServiceAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

            logger.Debug("PTService starter op");

            try
            {
                //JBS: Opsæt NLog og hent en logger instans til at logge opstart og fejl 
                var builder = WebApplication.CreateBuilder(args);

                //Ryd eksisterende logging providers og så bruger vi NLog i stedet for.
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();

                //JBS: Add services to the container.
                builder.Services.AddControllers();
                // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
                builder.Services.AddOpenApi();
                builder.Services.AddScoped<ISessionRepository, SessionRepository>();

                var app = builder.Build();

                //JBS: Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.MapOpenApi();
                    app.MapScalarApiReference(); //Scalar UI til API dokumentation
                }

                app.UseHttpsRedirection();
                app.UseAuthorization();
                app.MapControllers();
                app.Run();
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
