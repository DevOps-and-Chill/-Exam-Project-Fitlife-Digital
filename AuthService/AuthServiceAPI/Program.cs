using AuthServiceAPI.Data;
using AuthServiceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
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

//AO: Ensures DB and container exists. EnsureCreated() is alternative of migration
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();

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
