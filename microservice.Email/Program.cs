using MassTransit;
using microservice.Email;
using microservice.Email.Consumers;
using microservice.Email.Options;
using microservice.Email.Services;
using Microservice.Bus;
using Microservice.Bus.Events;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

// Email Settings Configuration
var emailSettings = builder.Configuration.GetSection("EmailSettings").Get<EmailSettings>()
    ?? throw new InvalidOperationException("EmailSettings configuration is missing");

// Override password from environment variable if present
var envPassword = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");
if (!string.IsNullOrEmpty(envPassword))
{
    emailSettings.Password = envPassword;
}

builder.Services.AddSingleton(emailSettings);

// Email Service
builder.Services.AddSingleton<EmailService>();

// MassTransit Configuration
builder.Services.AddMassTransit(x =>
{
    // Register consumers
    x.AddConsumer<OrderNotificationEventConsumer>();
    x.AddConsumer<DiscountNotificationEventConsumer>();

    x.UsingInMemory((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });

    // Kafka Configuration
    // Aspire'dan Kafka connection string'ini al
    var kafkaConnectionString = builder.Configuration.GetConnectionString("kafka");

    // Fallback to BusOptions if Aspire connection string is not available
    var busOptions = builder.Configuration.GetSection("BusOptions").Get<BusOptions>();
    var bootstrapServers = !string.IsNullOrEmpty(kafkaConnectionString)
        ? kafkaConnectionString
        : busOptions?.Host != null ? $"{busOptions.Host}:{busOptions.Port}" : "localhost:9094";

    x.AddRider(rider =>
    {
        // Order notification topic
        rider.AddConsumer<OrderNotificationEventConsumer>();
        rider.AddProducer<OrderCreatedNotificationEvent>("Order-created-notification-events");

        // Discount notification topic
        rider.AddConsumer<DiscountNotificationEventConsumer>();
        rider.AddProducer<DiscountNotificationEvent>("Discount-notification-events");

        rider.UsingKafka((context, k) =>
        {
            k.Host(bootstrapServers);

            k.TopicEndpoint<OrderCreatedNotificationEvent>("Order-created-notification-events", "email-service-group", e =>
            {
                // Servis kapalıyken gelen mesajları da işle
                e.AutoOffsetReset = Confluent.Kafka.AutoOffsetReset.Earliest;

                // Checkpoint interval - Her 10 mesajda bir offset kaydet
                e.CheckpointInterval = TimeSpan.FromSeconds(10);
                e.CheckpointMessageCount = 10;

                e.ConfigureConsumer<OrderNotificationEventConsumer>(context);
            });

            k.TopicEndpoint<DiscountNotificationEvent>("Discount-notification-events", "email-service-group", e =>
            {
                // Servis kapalıyken gelen mesajları da işle
                e.AutoOffsetReset = Confluent.Kafka.AutoOffsetReset.Earliest;

                // Checkpoint interval - Her 10 mesajda bir offset kaydet
                e.CheckpointInterval = TimeSpan.FromSeconds(10);
                e.CheckpointMessageCount = 10;

                e.ConfigureConsumer<DiscountNotificationEventConsumer>(context);
            });
        });
    });
});




var host = builder.Build();
host.Run();
