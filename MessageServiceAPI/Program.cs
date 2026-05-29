using Azure.Identity;
using MessageServiceAPI;
using MessageServiceAPI.Data;
using MessageServiceAPI.Extensions;
using MessageServiceAPI.Services;
using MessageServiceAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using System.Text;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("MessageService starter op");

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

    //AO: Config of Cosmos for EF
    builder.Services.AddDbContext<MessageDbContext>(options =>
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
    
    builder.Services.AddScoped<IMessageService, MessageService>();
    
    // Så ClassCancelledConsumer kun starter uden for development. Forhindrer run errors.
    if (!builder.Environment.IsDevelopment())
    {
        builder.Services.AddHostedService<ClassCancelledConsumer>();
    }
    
    

    var app = builder.Build();
    
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    
    using (var scope = app.Services.CreateScope())
    {

        var context = scope.ServiceProvider.GetRequiredService<MessageDbContext>();

        await context.Database.EnsureCreatedAsync();
    }
    
    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "MessageService stoppede på grund af en fejl!");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}