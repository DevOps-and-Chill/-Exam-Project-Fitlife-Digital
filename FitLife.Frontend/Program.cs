using FitLife.Frontend.Components;
using FitLife.Frontend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

//AO: Updated to follow nginx. Gateway route configed in appsettings, "nickname" is the name for the corresponding microservice.
//+ "[gateway-name]/" added to avoid changing calls in services
var gatewayBaseURL = builder.Environment.IsDevelopment() ? builder.Configuration["Gateway:BaseUrl"] : builder.Configuration["GatewayBaseUrl"];


	builder.Services.AddHttpClient("UserService", client =>
	{
		client.BaseAddress = new Uri(
			gatewayBaseURL + "user/");
	});

	builder.Services.AddHttpClient("FacilityService", client =>
	{
		client.BaseAddress = new Uri(
			 gatewayBaseURL + "facility/");
	});

	builder.Services.AddHttpClient("PTService", client =>
	{
		client.BaseAddress = new Uri(
			gatewayBaseURL + "pt/");
	});

	builder.Services.AddHttpClient("AuthService", client =>
	{
		client.BaseAddress = new Uri(
			  gatewayBaseURL + "auth/");
	});

	builder.Services.AddHttpClient("MessageService", client =>
	{
		client.BaseAddress = new Uri(
			gatewayBaseURL + "message/");
	});


	builder.Services.AddHttpClient("ClassService", client =>
	{
		client.BaseAddress = new Uri(
			gatewayBaseURL + "class/");
	});

	builder.Services.AddHttpClient("DigitalContentService", client =>
	{
		client.BaseAddress = new Uri(
			gatewayBaseURL + "content/");
	});

	builder.Services.AddHttpClient("StatisticService", client =>
	{
		client.BaseAddress = new Uri(
			gatewayBaseURL + "statistic/");
	});

builder.Services.AddMemoryCache();

builder.Services.AddScoped<ClassService>();
builder.Services.AddScoped<MemberService>();
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<DigitalContentService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CenterService>();
builder.Services.AddScoped<RegistrationStateService>();
builder.Services.AddScoped<PTService>();
builder.Services.AddScoped<CurrentUserService>();
builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<StatisticService>();
builder.Services.AddScoped<TokenService>();

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
