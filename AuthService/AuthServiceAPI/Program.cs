using AuthServiceAPI.Data;
using AuthServiceAPI.Extensions;
using AuthServiceAPI.Repositories;
using AuthServiceAPI.Repositories.Interfaces;
using AuthServiceAPI.Services;
using AuthServiceAPI.Services.Interfaces;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using System.Text;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

logger.Debug("AuthService starting");

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

    builder.Services.AddScoped<ICredentialRepository, CredentialRepositoryDB>();
    builder.Services.AddScoped<IPasswordService, PasswordService>();
    builder.Services.AddScoped<ICredentialService, CredentialService>();
    builder.Services.AddScoped<IJWTService, JWTService>();

    //AO: Config of Cosmos for EF
    builder.Services.AddDbContext<CredentialDbContext>(options =>
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

    //AO: Ensures DB and container exists. EnsureCreated() is alternative of migration
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<CredentialDbContext>();

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
    // Logger fejl hvis applikationen crasher ved opstart
    logger.Error(ex, "AuthService stoppede på grund af en fejl!");
    throw;
}
finally
{
    // Sørg for at alle logs bliver skrevet færdigt før applikationen lukker
    NLog.LogManager.Shutdown();
}