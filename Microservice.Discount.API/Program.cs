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

var app = builder.Build();
app.AddDiscountGroupEndpoints(app.AddVersionSetExt());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}


app.Run();

 