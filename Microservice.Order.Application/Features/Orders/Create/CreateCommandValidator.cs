using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Order.Application.Features.Orders.Create
{
    public class CreateCommandValidator : AbstractValidator<CreateOrderComand>
    {
        public CreateCommandValidator()
        {
            RuleFor(x => x.Orders)
                .NotNull().WithMessage("Orders cannot be null")
                .NotEmpty().WithMessage("At least one order item is required");

            RuleForEach(x => x.Orders).ChildRules(order =>
            {
                order.RuleFor(x => x.ProductId)
                    .NotEmpty().WithMessage("Product ID is required");

                order.RuleFor(x => x.ProdcutName)
                    .NotEmpty().WithMessage("Product name is required")
                    .MaximumLength(200).WithMessage("Product name cannot exceed 200 characters");

                order.RuleFor(x => x.UnitPrice)
                    .GreaterThan(0).WithMessage("Unit price must be greater than 0");
            });

            RuleFor(x => x.Discount)
                .InclusiveBetween(0f, 100f)
                .When(x => x.Discount.HasValue)
                .WithMessage("Discount must be between 0 and 100");

            RuleFor(x => x.Address)
                .NotNull().WithMessage("Address is required")
                .SetValidator(new AddressDtoValidator());

            RuleFor(x => x.Payment)
                .NotNull().WithMessage("Payment information is required")
                .SetValidator(new PaymentDtoValidator());
        }
    }

    public class AddressDtoValidator : AbstractValidator<AddressDto>
    {
        public AddressDtoValidator()
        {
            RuleFor(x => x.Province)
                .NotEmpty().WithMessage("Province is required")
                .MaximumLength(100).WithMessage("Province cannot exceed 100 characters");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required")
                .MaximumLength(100).WithMessage("City cannot exceed 100 characters");

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Street is required")
                .MaximumLength(200).WithMessage("Street cannot exceed 200 characters");

            RuleFor(x => x.PostalCode)
                .NotEmpty().WithMessage("Postal code is required")
                .MaximumLength(10).WithMessage("Postal code cannot exceed 10 characters");
        }
    }

    public class PaymentDtoValidator : AbstractValidator<PaymentDto>
    {
        public PaymentDtoValidator()
        {
            RuleFor(x => x.CardName)
                .NotEmpty().WithMessage("Cardholder name is required")
                .MaximumLength(100).WithMessage("Cardholder name cannot exceed 100 characters");

            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("Card number is required")
                .CreditCard().WithMessage("Invalid card number format")
                .MaximumLength(19).WithMessage("Card number cannot exceed 19 characters");

            RuleFor(x => x.Expiration)
                .NotEmpty().WithMessage("Expiration date is required")
                .Matches(@"^(0[1-9]|1[0-2])\/([0-9]{2})$")
                .WithMessage("Expiration date must be in MM/YY format");

            RuleFor(x => x.CVV)
                .NotEmpty().WithMessage("CVV is required")
                .Matches(@"^\d{3,4}$").WithMessage("CVV must be 3 or 4 digits");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Payment amount must be greater than 0");
        }
    }
}
