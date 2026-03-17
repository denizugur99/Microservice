using Microservice.Discount.API;
using Microservice.Discount.API.Features.Discount;
using Microservice.Discount.API.Options;
using Microservice.Discount.API.Repositories;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddVersionExt();
builder.Services.AddOptionsExt();
builder.Services.AddDatabaseExt();
builder.Services.AddCommonServiceExt(typeof(DiscountAssembly));
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

app.AddDiscountGroupEndpoints(app.AddVersionSetExt());

app.Run();

 