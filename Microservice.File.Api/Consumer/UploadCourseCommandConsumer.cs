using MassTransit;
using Microservice.Bus.Comnmands;
using Microservice.Bus.Events;
using Microsoft.Extensions.FileProviders;

namespace Microservice.File.Api.Consumer
{
    public class UploadCourseCommandConsumer(IServiceProvider serviceProvider) : IConsumer<UploadCoursePictureCommand>
    {
  

        public async Task Consume(ConsumeContext<UploadCoursePictureCommand> context)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var provider = scope.ServiceProvider.GetRequiredService<IFileProvider>();

                var newFileName = $"{Guid.NewGuid()}{Path.GetExtension(context.Message.fileName)}";
                var uploadPath = Path.Combine(provider.GetFileInfo("files").PhysicalPath!, newFileName);
               
                await System.IO.File.WriteAllBytesAsync(uploadPath, context.Message.picture);
                var publish = scope.ServiceProvider.GetRequiredService<ITopicProducer<CoursePictureUploadedEvent>>();
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var fileApiBaseUrl = configuration["FileApiBaseUrl"] ?? "http://localhost:5237";
                await publish.Produce(new CoursePictureUploadedEvent(context.Message.courseId,$"{fileApiBaseUrl}/files/{newFileName}"));
            }
           

        }
    }
}

