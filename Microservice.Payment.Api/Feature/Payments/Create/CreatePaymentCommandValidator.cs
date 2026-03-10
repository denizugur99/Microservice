using FluentValidation;

namespace Microservice.Payment.Api.Feature.Payments.Create
{
    public class CreatePaymentCommandValidator:AbstractValidator<CreatePaymentCommand>
    {
        public CreatePaymentCommandValidator() {
        RuleFor(x => x.OrderCode)
            .NotEmpty().WithMessage("Order code is required.");

        RuleFor(x => x.CardNumber)
            .NotEmpty().WithMessage("Card number is required.")
            .CreditCard().WithMessage("Invalid card number format.");

        RuleFor(x => x.CardHolderName)
            .NotEmpty().WithMessage("Card holder name is required.")
            .MinimumLength(3).WithMessage("Card holder name must be at least 3 characters.")
            .MaximumLength(100).WithMessage("Card holder name cannot exceed 100 characters.");

        RuleFor(x => x.Expiration)
            .NotEmpty().WithMessage("Expiration date is required.")
            .Matches(@"^(0[1-9]|1[0-2])\/([0-9]{2})$").WithMessage("Expiration date must be in MM/YY format.");

        RuleFor(x => x.Cvv)
            .NotEmpty().WithMessage("CVV is required.")
            .Matches(@"^[0-9]{3,4}$").WithMessage("CVV must be 3 or 4 digits.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0.");

        }
    }
}
