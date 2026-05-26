using FitLife.Frontend.Components;
using FitLife.Frontend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

//AO: Updated to follow nginx. Gateway route configed in appsettings, "nickname" is the name for the corresponding microservice.
//+ "[gateway-name]/" added to avoid changing calls in services
builder.Services.AddHttpClient("UserService", client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["Gateway:BaseUrl"]! + "user/"); 
});

builder.Services.AddHttpClient("FacilityService", client =>
{
    client.BaseAddress = new Uri(
         builder.Configuration["Gateway:BaseUrl"]! + "facility/"); 
});

builder.Services.AddHttpClient("PTService", client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["Gateway:BaseUrl"]! + "pt/");
});

builder.Services.AddHttpClient("AuthService", client =>
{
    client.BaseAddress = new Uri(
          builder.Configuration["Gateway:BaseUrl"]! + "auth/");
});

builder.Services.AddHttpClient("MessageService", client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["Gateway:BaseUrl"]! + "message/");
});


builder.Services.AddHttpClient("ClassService",client =>
{
    client.BaseAddress =new Uri(
        builder.Configuration["Gateway:BaseUrl"]! + "class/");
});

//AO: Not yet added to nginx.conf
builder.Services.AddHttpClient("DigitalContentService", client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["Gateway:BaseUrl"]! + "content/");
});

builder.Services.AddMemoryCache();

builder.Services.AddHttpClient("StatisticService", client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["StatisticService:BaseUrl"]!);
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
builder.Services.AddScoped<StatisticService>();

builder.Services.AddSingleton<TokenService>();

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
