using MassTransit;
using Microservice.Bus;
using Microservice.Bus.Comnmands;
using Microservice.Bus.Events;
using Microservice.Discount.API.Features.Discount;
using Microservice.File.Api;
using Microservice.File.Api.Consumer;
using Microservices.Shared.Extensions;
using Microsoft.Extensions.FileProviders;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddVersionExt();
builder.Services.AddCommonServiceExt(typeof(FileAssembly));

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
        // Consumer: UploadCoursePictureCommand dinle
        rider.AddConsumer<UploadCourseCommandConsumer>();

        // Producer: CoursePictureUploadedEvent gönder
        rider.AddProducer<CoursePictureUploadedEvent>("course-picture-uploaded");

        rider.UsingKafka((context, kafka) =>
        {
            kafka.Host(busOptions.BootstrapServers); // localhost:9094

            // Mesaj boyutu - Kafka broker limiti ile uyumlu (KAFKA_MESSAGE_MAX_BYTES=10MB)
            kafka.MessageMaxBytes = 10 * 1024 * 1024;

            // Topic dinle: Order-events
            kafka.TopicEndpoint<UploadCoursePictureCommand>(
                "Order-events",                 // Topic adı
                "file-service-group",           // Consumer group
                e =>
                {
                    e.ConfigureConsumer<UploadCourseCommandConsumer>(context);

                    // Kapalıyken gelen mesajları oku
                    e.AutoOffsetReset = Confluent.Kafka.AutoOffsetReset.Earliest;

                    // Mesaj işlendikten HEMEN sonra commit et (kuyruktan sil)
                    e.CheckpointInterval = TimeSpan.FromSeconds(1);
                    e.CheckpointMessageCount = 1; // Her 1 mesajda commit
                });
        });
    });
});

builder.Services.AddSingleton<IFileProvider>( new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
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
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.AddFileGroupEndpoints(app.AddVersionSetExt());


app.Run();


