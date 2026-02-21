using FluentValidation;

namespace Microservice.Basket.Api.Features.Baskets.AddBasketItem
{
    public class AddBasketItemValidator:AbstractValidator<AddBasketItemCommand>
    {
        public AddBasketItemValidator() {
            RuleFor(x => x.CourseId).NotEmpty().WithMessage("CourseId is required");
            RuleFor(x => x.CourseName).NotEmpty().WithMessage("CourseName is required");
            RuleFor(x => x.CoursePrice).GreaterThan(0).WithMessage("{PropertyName} must be greate than zero");
           
        }
    }
}
