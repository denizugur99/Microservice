namespace Microservice.Catalog.Api.Features.Courses.Create
{
    public class CreateCourseComaandValidator:AbstractValidator<CreateCourseCommand>
    {
        // Kafka broker mesaj limiti 10MB (docker-compose.yml KAFKA_MESSAGE_MAX_BYTES) ile uyumlu, güvenlik payı bırakılmış boyut
        public const long MaxPictureSizeBytes = 5 * 1024 * 1024;

        public CreateCourseComaandValidator()
        {
            RuleFor(x => x.name).NotEmpty().WithMessage("Name is required.").MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters");
            RuleFor(x => x.description).NotEmpty().WithMessage("Description is required.").MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters");
            RuleFor(x => x.price).GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.categoryId).NotEmpty().WithMessage("Category ID is required.");

            RuleFor(x => x.picture)
                .Must(picture => picture == null || picture.Length <= MaxPictureSizeBytes)
                .WithMessage($"Course picture must not exceed {MaxPictureSizeBytes / (1024 * 1024)}MB.");
        }
    }
}
