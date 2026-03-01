namespace Microservice.Discount.API.Features.Discount.CreateDiscount
{
    public class CreateDiscountCommandValidator:AbstractValidator<CreateDiscountCommand>
    {
        public CreateDiscountCommandValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage("{PropertyName} is required.").Length(10).WithErrorCode("{PropertyName} must be 10 characters long.");
            RuleFor(x => x.Rate).NotEmpty().WithMessage("{PropertyName} is required.");
            RuleFor(x => x.UserId).NotEmpty().WithMessage("{PropertyName} is required.");
            RuleFor(x => x.ExpireDate).GreaterThan(DateTimeOffset.Now).WithMessage("ExpireDate must be in the future.");
        }
    }
}
