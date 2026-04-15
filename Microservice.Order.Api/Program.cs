using Microservice.Bus;
using Microservice.Order.Api;
using Microservice.Order.Api.Endpoints.Orders;
using Microservice.Order.Application;
using Microservice.Order.Application.BackgroundServices;
using Microservice.Order.Application.Contracts.Refit;
using Microservice.Order.Application.Contracts.Refit.PaymentService;
using Microservice.Order.Application.Contracts.Repositories;
using Microservice.Order.Application.Contracts.UnitOfWorks;
using Microservice.Order.Persistence;
using Microservice.Order.Persistence.Repositories;
using Microservice.Order.Persistence.UnitOfWork;
using Microservices.Shared.Extensions;
using Microservices.Shared.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Refit;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCommonServiceExt(typeof(OrderApplicationAssembly));
builder.Services.AddMassTransitExt(builder.Configuration);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});
builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
builder.Services.AddVersionExt();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthenticationExt(builder.Configuration);
builder.Services.AddScoped<AuthenticatedHttpClientHandler>();
builder.Services.AddScoped<ClientAuthenticatedHttpClientHandler>();
builder.Services.AddRefitClient<IPaymentService>().ConfigureHttpClient(cfg =>
{
    var addressUrlOption=builder.Configuration.GetSection(nameof(AddressUrlOption)).Get<AddressUrlOption>();
    cfg.BaseAddress = new Uri(addressUrlOption!.PaymentUrl);
}).AddHttpMessageHandler<AuthenticatedHttpClientHandler>().AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();

builder.Services.AddOptions<IdentityOption>().BindConfiguration(nameof(IdentityOption)).ValidateDataAnnotations().ValidateOnStart();
builder.Services.AddSingleton<IdentityOption>(sp=>sp.GetRequiredService<IOptions<IdentityOption>>().Value);
builder.Services.AddOptions<ClientSecretOptions>().BindConfiguration(nameof(ClientSecretOptions)).ValidateDataAnnotations().ValidateOnStart();
builder.Services.AddSingleton<ClientSecretOptions>(sp => sp.GetRequiredService<IOptions<ClientSecretOptions>>().Value);
builder.Services.AddHostedService<CheckPaymentStatus>();



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

app.AddOrderGroupEndpoints(app.AddVersionSetExt());

app.Run();

