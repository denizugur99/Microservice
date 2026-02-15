namespace Microservice.Catalog.Api.Features.Courses.Create
{
    public class CreateCourseComaandValidator:AbstractValidator<CreateCourseCommand>
    {
        public CreateCourseComaandValidator()
        {
            RuleFor(x => x.name).NotEmpty().WithMessage("Name is required.").MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters");
            RuleFor(x => x.description).NotEmpty().WithMessage("Description is required.").MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters");
            RuleFor(x => x.price).GreaterThan(0).WithMessage("Price must be greater than zero.");
           
            RuleFor(x => x.categoryId).NotEmpty().WithMessage("Category ID is required.");
        }
    }
}
