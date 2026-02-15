

namespace Microservice.Catalog.Api.Features.Categories.Create
{
    public class CreateCategortCommandValidator:AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategortCommandValidator() {
        
        RuleFor(x => x.categoryName).
                NotEmpty().WithMessage("Name is required").
                Length(4,35).WithMessage("Name must be less than 35 characters");


        }
    }
}
