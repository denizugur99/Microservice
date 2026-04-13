using MassTransit;
using Microservice.Basket.Api;
using Microservice.Basket.Api.Consumer;
using Microservice.Basket.Api.Features.Baskets;
using Microservice.Bus;
using Microservice.Bus.Events;
using Microservices.Shared.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCommonServiceExt(typeof(BasketAssembly));

// MassTransit + Kafka Consumer
var busOptions = builder.Configuration.GetSection(nameof(BusOptions)).Get<BusOptions>() ?? new BusOptions();
builder.Services.AddMassTransit(x =>
{
    x.UsingInMemory((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });

    x.AddRider(rider =>
    {
        // Producer: Topic'i oluşturmak için (mesaj göndermese de topic oluşur)
        rider.AddProducer<OrderCreatedEvent>("Order-created-events");

        // Consumer: OrderCreatedEvent dinle
        rider.AddConsumer<OrderCreatedEventConsumer>();

        rider.UsingKafka((context, kafka) =>
        {
            kafka.Host(busOptions.BootstrapServers); // localhost:9094

            // Mesaj boyutu - 10MB
            kafka.MessageMaxBytes = 10 * 1024 * 1024;

            // Topic dinle: order-created-events
            kafka.TopicEndpoint<OrderCreatedEvent>(
                "Order-created-events",      // "order-created-events"
                "basket-service-group",   // "basket-service-group"
                e =>
                {
                    e.ConfigureConsumer<OrderCreatedEventConsumer>(context);

                    // Kapalıyken gelen mesajları oku
                    e.AutoOffsetReset = Confluent.Kafka.AutoOffsetReset.Earliest;

                    // Mesaj işlendikten HEMEN sonra commit et
                    e.CheckpointInterval = TimeSpan.FromSeconds(1);
                    e.CheckpointMessageCount = 1;
                });
        });
    });
});

builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration = builder.Configuration.GetConnectionString("Redis");
});
builder.Services.AddScoped<BasketService>();
builder.Services.AddVersionExt();
builder.Services.AddAuthenticationExt(builder.Configuration);
var app = builder.Build();

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

app.AddBasketGroupEndpoints(app.AddVersionSetExt());

app.Run();

