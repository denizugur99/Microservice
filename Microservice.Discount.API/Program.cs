using MassTransit;
using Microservice.Bus;
using Microservice.Bus.Events;
using Microservice.Discount.API;
using Microservice.Discount.API.Consumer;
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

            // Topic dinle: Order-created-events
            kafka.TopicEndpoint<OrderCreatedEvent>(
                "Order-created-events",
                "discount-service-group",
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

 