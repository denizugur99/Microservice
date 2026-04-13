using MicroserviceWebApp.DelegatedHandlers;
using MicroserviceWebApp.ExceptionHandlers;
using MicroserviceWebApp.Extensions;
using MicroserviceWebApp.Options;
using MicroserviceWebApp.Options.GatewayOptions;
using MicroserviceWebApp.Pages.Auth.SignIn;
using MicroserviceWebApp.Pages.Auth.SignUp;
using MicroserviceWebApp.Services;
using MicroserviceWebApp.Services.Refit;
using Microsoft.AspNetCore.Authentication.Cookies;
using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddMvc(opt=>opt.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes=true);
builder.Services.AddOptionsExt();

builder.Services.AddHttpClient<SignUpService>();
builder.Services.AddHttpClient<SignInService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<CatalogService>();
builder.Services.AddScoped<BasketService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthenticatedHttpClientHandler>();
builder.Services.AddScoped<ClientAuthenticatedHttpClientHandler>();
builder.Services.AddExceptionHandler<UnauthorizedExceptionHandler>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddRefitClient<ICatalogRefitService>().ConfigureHttpClient((sp, config) =>
{
    var microserviceOption = sp.GetRequiredService<MicroserviceOptions>();
    config.BaseAddress = new Uri(microserviceOption.Catalog.BaseAddress);
}).AddHttpMessageHandler<AuthenticatedHttpClientHandler>().AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();

builder.Services.AddRefitClient<IBasketRefitService>().ConfigureHttpClient((sp, config) =>
{
    var microserviceOption = sp.GetRequiredService<MicroserviceOptions>();
    config.BaseAddress = new Uri(microserviceOption.Basket.BaseAddress);
}).AddHttpMessageHandler<AuthenticatedHttpClientHandler>();



builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
{
    opt.LoginPath = "/Auth/SignIn";
    opt.ExpireTimeSpan=TimeSpan.FromDays(60);
    opt.Cookie.Name="MicroserviceWebAppAuthCookie";
    opt.AccessDeniedPath= "/Auth/AccessDenied";
});
builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
