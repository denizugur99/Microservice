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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOptionsExt();
builder.Services.AddDatabaseExt();

// MassTransit + Kafka (Producer + Consumer)
var busOptions = builder.Configuration.GetSection(nameof(BusOptions)).Get<BusOptions>() ?? new BusOptions();
builder.Services.AddMassTransit(x =>
{
    x.UsingInMemory((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });

    x.AddRider(rider =>
    {
        // Producer: UploadCoursePictureCommand gönder
        rider.AddProducer<UploadCoursePictureCommand>("course-picture-uploaded");

        // Consumer: CoursePictureUploadedEvent dinle
        rider.AddConsumer<CoursePictureUploadedEventConsumer>();

        rider.UsingKafka((context, kafka) =>
        {
            kafka.Host(busOptions.BootstrapServers); // localhost:9094

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
