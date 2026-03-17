using Microservice.Basket.Api;
using Microservice.Basket.Api.Features.Baskets;
using Microservices.Shared.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCommonServiceExt(typeof(BasketAssembly));
builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration = builder.Configuration.GetConnectionString("Redis");
});
builder.Services.AddScoped<BasketService>();
builder.Services.AddVersionExt();
builder.Services.AddAuthenticationExt(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.AddBasketGroupEndpoints(app.AddVersionSetExt());

app.Run();

