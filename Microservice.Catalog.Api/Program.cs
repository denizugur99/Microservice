using Microservice.Catalog.Api;
using Microservice.Catalog.Api.Features.Categories;
using Microservice.Catalog.Api.Features.Courses;
using Microservice.Catalog.Api.Options;
using Microservices.Shared.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOptionsExt();
builder.Services.AddDatabaseExt();
builder.Services.AddCommonServiceExt(typeof(CatalogAssembly));
builder.Services.AddVersionExt();
builder.Services.AddAuthenticationExt(builder.Configuration);


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();
app.AddSeedDataExt().ContinueWith(x =>
{
    if (x.IsFaulted)
        Console.WriteLine(x.Exception?.Message);
    else
    {
        Console.WriteLine("Seed data has been saved to DB");
    }
});

// Configure the HTTP request pipeline.
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{

    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.AddCategoryGroupEndpoints(app.AddVersionSetExt());
app.AddCourseGroupEndpoints(app.AddVersionSetExt());



app.Run();
