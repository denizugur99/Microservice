using Microservice.Bus.Events;

namespace Microservice.Catalog.Api.Consumer
{
    public class CoursePictureUploadedEventConsumer(IServiceProvider serviceProvider) : IConsumer<CoursePictureUploadedEvent>
    {
        public async Task Consume(ConsumeContext<CoursePictureUploadedEvent> context)
        {
            using(var scope = serviceProvider.CreateScope())
            {
                var provider = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var course = provider.Courses.FirstOrDefault(c => c.Id == context.Message.CourseId);
                if (course == null) { throw new NotImplementedException(); }


                    course.ImageUrl = context.Message.ImageUrl;
                   await  provider.SaveChangesAsync();

               
                
            }
        }
    }
}
