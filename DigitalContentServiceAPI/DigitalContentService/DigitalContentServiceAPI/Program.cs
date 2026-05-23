
using DigitalContentServiceAPI.Data;
using DigitalContentServiceAPI.Repositories;
using DigitalContentServiceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalContentServiceAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<IWorkoutProgramRepository, WorkoutProgramRepository>();
            builder.Services.AddScoped<IWorkoutVideoRepository, WorkoutVideoRepository>();
            
            builder.Services.AddDbContext<DigitalContentDbContext>(options =>
                options.UseCosmos(
                    builder.Configuration["CosmosDb:AccountEndpoint"],
                    builder.Configuration["CosmosDb:AccountKey"],
                    builder.Configuration["CosmosDb:DatabaseName"]
                ));

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

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
