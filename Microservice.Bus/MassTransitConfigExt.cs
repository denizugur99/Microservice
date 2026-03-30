using MassTransit;
using Microservice.Bus.Comnmands;
using Microservice.Bus.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Microservice.Bus
{
    public static class MassTransitConfigExt
    {
        public static IServiceCollection AddMassTransitExt(this IServiceCollection services, IConfiguration configuration)
        {
            var busOptions = (configuration.GetSection(nameof(BusOptions)).Get<BusOptions>())!;

            services.AddMassTransit(configure =>
            {
                configure.UsingInMemory((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });

                configure.AddRider(rider =>
                {
                    rider.AddProducer<CoursePictureUploadedEvent>("Picture-uploaded-events");
                    rider.AddProducer<UploadCoursePictureCommand>("Order-events");
                    rider.AddProducer<OrderCreatedEvent>("Order-created-events");


                    rider.UsingKafka((context, kafka) =>
                    {
                        kafka.Host(busOptions.BootstrapServers);

                        // Producer mesaj boyutu limiti - 10MB
                        kafka.MessageMaxBytes = 10 * 1024 * 1024; // 10 MB
                    });
                });
            });

            return services;
        }
    }
}
