
using UserServiceAPI.Repositories;
using UserServiceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using UserServiceAPI.Data;

namespace UserServiceAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<IUserRepository, UserRepositoryDB>();
            builder.Services.AddScoped<IMemberRepository, MemberRepositoryDB>();
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepositoryDB>();

            //AO: Accountkey and endpoint to be added to vault currently in appsettings
            builder.Services.AddDbContext<UserDbContext>(options =>
            {
                options.UseCosmos(
                builder.Configuration["CosmosDb:AccountEndpoint"]!,
                builder.Configuration["CosmosDb:AccountKey"]!,
                builder.Configuration["CosmosDb:DatabaseName"]!);
            });
        

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(
                new System.Text.Json.Serialization.JsonStringEnumConverter());
            });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            //AO: Ensures DB and container exists. EnsureCreated() is alternative of migration
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<UserDbContext>();

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
