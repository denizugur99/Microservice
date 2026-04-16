using MassTransit;
using Microservice.Bus;
using Microservice.Bus.Comnmands;
using Microservice.Bus.Events;
using Microservice.Catalog.Api;
using Microservice.Catalog.Api.Consumer;
using Microservice.Catalog.Api.Features.Categories;
using Microservice.Catalog.Api.Features.Courses;
using Microservice.Catalog.Api.Options;
using Microservices.Shared.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOptionsExt();
builder.Services.AddDatabaseExt();

// MassTransit + Kafka (Producer + Consumer)
// Aspire'dan Kafka connection string'ini al
var kafkaConnectionString = builder.Configuration.GetConnectionString("kafka");

// Fallback to BusOptions if Aspire connection string is not available
var busOptions = builder.Configuration.GetSection(nameof(BusOptions)).Get<BusOptions>();
var bootstrapServers = !string.IsNullOrEmpty(kafkaConnectionString)
    ? kafkaConnectionString
    : busOptions?.BootstrapServers ?? "localhost:9094";

builder.Services.AddMassTransit(x =>
{
    // MassTransit 9.x lisans ayarı - development için
    // Production ortam için https://masstransit.io/ adresinden ücretsiz lisans alınmalı
    x.SetKebabCaseEndpointNameFormatter();

    x.UsingInMemory((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });

    x.AddRider(rider =>
    {
        // Producer: UploadCoursePictureCommand gönder (File API ile aynı topic)
        rider.AddProducer<UploadCoursePictureCommand>("Order-events");

        // Consumer: CoursePictureUploadedEvent dinle
        rider.AddConsumer<CoursePictureUploadedEventConsumer>();

        rider.UsingKafka((context, kafka) =>
        {
            kafka.Host(bootstrapServers);

            // Mesaj boyutu - 10MB
            kafka.MessageMaxBytes = 10 * 1024 * 1024;

            // Topic dinle: course-picture-uploaded
            kafka.TopicEndpoint<CoursePictureUploadedEvent>(
                "course-picture-uploaded",
                "catalog-service-group",
                e =>
                {
                    e.ConfigureConsumer<CoursePictureUploadedEventConsumer>(context);

                    // Kapalıyken gelen mesajları oku
                    e.AutoOffsetReset = Confluent.Kafka.AutoOffsetReset.Earliest;

               
                });
        });
    });
});

builder.Services.AddCommonServiceExt(typeof(CatalogAssembly));
builder.Services.AddVersionExt();
builder.Services.AddAuthenticationExt(builder.Configuration);


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

app.AddSeedDataExt().ContinueWith(x =>
{
    if (x.IsFaulted)
        Console.WriteLine(x.Exception?.Message);
    else
    {
        Console.WriteLine("Seed data has been saved to DB");
    }
});

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

app.AddCategoryGroupEndpoints(app.AddVersionSetExt());
app.AddCourseGroupEndpoints(app.AddVersionSetExt());



app.Run();
