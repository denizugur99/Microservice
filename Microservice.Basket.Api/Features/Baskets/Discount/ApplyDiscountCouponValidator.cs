using FluentValidation;

namespace Microservice.Basket.Api.Features.Baskets.Discount
{
    public class ApplyDiscountCouponValidator : AbstractValidator<ApplyDiscountCouponCommand>
    {
        public ApplyDiscountCouponValidator()
        {
            RuleFor(x => x.Coupon)
                .NotEmpty().WithMessage("Coupon code is required")
                .MinimumLength(3).WithMessage("Coupon code must be at least 3 characters");

            RuleFor(x => x.Rate)
                .GreaterThan(0).WithMessage("Discount rate must be greater than 0")
                .LessThanOrEqualTo(1).WithMessage("Discount rate cannot exceed 100% (1.0)");
        }
    }
}
