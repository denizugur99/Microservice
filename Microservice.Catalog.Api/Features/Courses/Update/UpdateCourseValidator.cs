namespace Microservice.Catalog.Api.Features.Courses.Update
{
    public class UpdateCourseValidator:AbstractValidator<UpdateCourseCommand>
    {
        public UpdateCourseValidator() {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.").MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.").MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category ID is required.");
        }
    }
}
