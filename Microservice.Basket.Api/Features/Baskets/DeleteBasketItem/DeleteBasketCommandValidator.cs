using FluentValidation;

namespace Microservice.Basket.Api.Features.Baskets.DeleteBasketItem
{
    public class DeleteBasketCommandValidator:AbstractValidator<DeleteBasketItemCommand>
    {
        public DeleteBasketCommandValidator() {
            RuleFor(x => x.courseId).NotEmpty().WithMessage("courseıd is missing");
        }
    }
}
