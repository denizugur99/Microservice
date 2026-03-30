using Microservice.Bus.Comnmands;

namespace Microservice.Catalog.Api.Features.Courses.Create
{
    public class CreateCourseCommandHandler(AppDbContext context,IMapper mapper,IServiceProvider serviceProvider) : IRequestHandler<CreateCourseCommand, ServiceResult<Guid>>
    {
        public async Task<ServiceResult<Guid>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            var hasCategory = await context.Categories.AnyAsync(c => c.Id == request.categoryId, cancellationToken);
            if (!hasCategory) { 
            return ServiceResult<Guid>.Error("Not Found", $"Category with id {request.categoryId} not found", HttpStatusCode.NotFound);
            }
            var hascourseWithSameName = await context.Courses.AnyAsync(c => c.Name == request.name, cancellationToken);
            if (hascourseWithSameName) {
            return ServiceResult<Guid>.Error("Conflict", $"Course with name {request.name} already exists", HttpStatusCode.BadRequest);
            }


            var newCourse = mapper.Map<Course>(request);
            newCourse.Id=NewId.NextSequentialGuid();
            newCourse.CreatedDate = DateTimeOffset.UtcNow;
            newCourse.Feature=new Feature
            {
                Duration=10, //calculation pending
                EducatorFullName="John Doe", //tokendname pending
                Rating =0
            };
            context.Courses.Add(newCourse);
            await context.SaveChangesAsync(cancellationToken);

            if (request.Picture is not null)
            {
                using var memoryStream = new MemoryStream();

                await request.Picture.CopyToAsync(memoryStream, cancellationToken);
                var PictureAsByteArray=memoryStream.ToArray();
                var  uploadCoursePictureCommand = new UploadCoursePictureCommand(newCourse.Id, PictureAsByteArray,request.Picture.FileName);

                // Scope oluştur ve producer'ı al
                using var scope = serviceProvider.CreateScope();
                var producer = scope.ServiceProvider.GetRequiredService<ITopicProducer<UploadCoursePictureCommand>>();
                await producer.Produce(uploadCoursePictureCommand, cancellationToken);

            }
            return ServiceResult<Guid>.SuccesAsCreated(newCourse.Id, $"/api/courses/{newCourse.Id}");

        }
    }
}
