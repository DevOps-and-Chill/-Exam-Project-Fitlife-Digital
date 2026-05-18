
using FacilityServiceAPI.Contexts;
using FacilityServiceAPI.Repositories;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace FacilityServiceAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
			builder.Services.AddOpenApi();

			builder.Services.AddTransient<IFacilityRepository, FacilityRepositoryMoq>();

			builder.Services.AddDbContextFactory<FacilityContext>(options => options.UseCosmos(
				builder.Configuration.GetConnectionString("CosmosDBConn"),
				builder.Configuration.GetValue("Databasename",string.Empty)));

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
