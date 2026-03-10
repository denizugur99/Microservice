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

var app = builder.Build();

app.AddPaymentGroupEndpoints(app.AddVersionSetExt());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}



app.Run();


