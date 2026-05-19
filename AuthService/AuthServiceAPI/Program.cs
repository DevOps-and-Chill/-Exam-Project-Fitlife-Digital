using NLog;
using NLog.Web;
using AuthServiceAPI.Data;
using AuthServiceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

logger.Debug("AuthService starter op");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Ryd eksisterende logging providers og brug NLog i stedet
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddControllers();
    builder.Services.AddOpenApi();

    builder.Services.AddScoped<IAuthRepository, AuthRepositoryDB>();

    builder.Services.AddDbContext<AuthDbContext>(options =>
    {
        options.UseCosmos(
            builder.Configuration["CosmosDb:AccountEndpoint"]!,
            builder.Configuration["CosmosDb:AccountKey"]!,
            builder.Configuration["CosmosDb:DatabaseName"]!
        );
    });

    var app = builder.Build();

    // Sikrer at DB og container eksisterer
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
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
    logger.Error(ex, "AuthService stoppede på grund af en fejl!");
    throw;
}
finally
{
    // Sørg for at alle logs bliver skrevet færdigt før applikationen lukker
    NLog.LogManager.Shutdown();
}