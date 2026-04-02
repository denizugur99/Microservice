using Microservice.Bus;
using Microservice.Payment.Api;
using Microservice.Payment.Api.Feature.Payments;
using Microservice.Payment.Api.Repositories;
using Microservices.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddVersionExt();
builder.Services.AddCommonServiceExt(typeof(PaymentAssembly));

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("PaymentDb");
});
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

app.AddPaymentGroupEndpoints(app.AddVersionSetExt());


app.Run();


