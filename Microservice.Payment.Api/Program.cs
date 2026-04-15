using Microservice.Bus;
using Microservice.Payment.Api;
using Microservice.Payment.Api.Feature.Payments;
using Microservice.Payment.Api.Repositories;
using Microservices.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

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

app.MapDefaultEndpoints();

// Global exception handler
app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";

        var exceptionHandlerFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        var exception = exceptionHandlerFeature?.Error;

        var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An error occurred while processing your request",
            Detail = app.Environment.IsDevelopment() ? exception?.Message : "An internal server error occurred",
            Instance = context.Request.Path
        };

        await context.Response.WriteAsJsonAsync(problemDetails);
    });
});

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


