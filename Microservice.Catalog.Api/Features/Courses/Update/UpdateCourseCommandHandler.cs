namespace Microservice.Catalog.Api.Features.Courses.Update
{
    public class UpdateCourseCommandHandler(AppDbContext context,IMapper mapper) : IRequestHandler<UpdateCourseCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
           var course=await context.Courses.FindAsync(request.Id,cancellationToken);
            if (course == null) {
                return ServiceResult.ErrorAsNotFound();
            }
            course.Name = request.Name;
            course.Description = request.Description;
            course.Price = request.Price;
            if (!string.IsNullOrEmpty(request.ImageUrl)) {
                course.ImageUrl = request.ImageUrl;
            }

           
            course.CategoryId = request.CategoryId;
            context.Courses.Update(course);
            await context.SaveChangesAsync(cancellationToken);
            return ServiceResult.SuccesAsNoContent();

        }
    }
}
