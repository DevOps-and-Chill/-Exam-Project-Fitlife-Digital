using ClassServiceAPI.Data;
using ClassServiceAPI.Messaging;
using ClassServiceAPI.Repositories;
using ClassServiceAPI.Repositories.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IClassRepository, ClassRepository>();

builder.Services.AddDbContext<ClassDbContext>(options =>
    options.UseCosmos(
        accountEndpoint: "https://localhost:8081/",
        accountKey: "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
        databaseName: "FitLife"
    )
);

builder.Services.AddSingleton<IMessagePublisher>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return RabbitMqPublisher.CreateAsync(config).GetAwaiter().GetResult();
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ClassDbContext>();
    await context.Database.EnsureCreatedAsync();
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
