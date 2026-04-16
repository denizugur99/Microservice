using MassTransit;
using Microservice.Bus;
using Microservice.Bus.Comnmands;
using Microservice.File.Api.Consumer;

namespace Microservice.File.Api.Extensions
{
    public static class MassTransitFileExt
    {
        public static IServiceCollection AddMassTransitWithKafkaExt(this IServiceCollection services, IConfiguration configuration)
        {
            // Aspire'dan Kafka connection string'ini al
            var kafkaConnectionString = configuration.GetConnectionString("kafka");

            // Fallback to BusOptions if Aspire connection string is not available
            var busOptions = configuration.GetSection(nameof(BusOptions)).Get<BusOptions>();
            var bootstrapServers = !string.IsNullOrEmpty(kafkaConnectionString)
                ? kafkaConnectionString
                : busOptions?.BootstrapServers ?? "localhost:9094";

            services.AddMassTransit(configure =>
            {
                configure.UsingInMemory((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });

                configure.AddRider(rider =>
                {
                    // Consumer'ı kaydet
                    rider.AddConsumer<UploadCourseCommandConsumer>();

                    rider.UsingKafka((context, kafka) =>
                    {
                        kafka.Host(bootstrapServers);

                        // Consumer mesaj boyutu limiti - 10MB
                        kafka.MessageMaxBytes = 10 * 1024 * 1024;

                        // Topic'i dinle
                        kafka.TopicEndpoint<UploadCoursePictureCommand>(
                            "Order-events",                     // Topic adı (Producer ile aynı)
                            "file-service-group",               // Consumer group
                            e =>
                            {
                                e.ConfigureConsumer<UploadCourseCommandConsumer>(context);
                            });
                    });
                });
            });

            return services;
        }
    }
}
