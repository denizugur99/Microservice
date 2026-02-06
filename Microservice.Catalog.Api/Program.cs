using Microservice.Catalog.Api.Options;
using Microservice.Catalog.Api.Repositories;
using MongoDB.Driver;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOptionsExt();
builder.Services.AddDatabaseExt();


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
    app.MapOpenApi();
    app.MapScalarApiReference();
}



app.Run();
