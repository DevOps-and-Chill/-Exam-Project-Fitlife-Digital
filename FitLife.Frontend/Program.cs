using FitLife.Frontend.Components;
using FitLife.Frontend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

    builder.Services.AddHttpClient("UserService", client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["UserService:BaseUrl"]!);
});

builder.Services.AddHttpClient("FacilityService", client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["FacilityService:BaseUrl"]!);
});

builder.Services.AddHttpClient("PTService", client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["PTService:BaseUrl"]!);
});

builder.Services.AddHttpClient("AuthService", client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["AuthService:BaseUrl"]!);
});

builder.Services.AddHttpClient("MessageService", client =>
{
    client.BaseAddress = new Uri("http://localhost:5001");
});

builder.Services.AddScoped<SessionService>();
builder.Services.AddScoped<MemberService>();
builder.Services.AddScoped<TrainerService>();
builder.Services.AddScoped<DigitalTrainingService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CenterService>();
builder.Services.AddScoped<RegistrationStateService>();
builder.Services.AddScoped<PTService>();
builder.Services.AddScoped<CurrentUserService>();
builder.Services.AddScoped<MessageService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
