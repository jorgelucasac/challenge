using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class SaleItemValidator : AbstractValidator<SaleItem>
{
    public SaleItemValidator()
    {
        RuleFor(item => item.ProductExternalId)
            .NotEmpty()
            .WithMessage("Product external id is required.");

        RuleFor(item => item.ProductName)
            .NotEmpty()
            .WithMessage("Product name is required.");

        RuleFor(item => item.Quantity)
            .GreaterThan(0)
            .WithMessage("Item quantity must be greater than zero.")
            .LessThanOrEqualTo(20)
            .WithMessage("It is not possible to sell more than 20 identical items.");

        RuleFor(item => item.UnitPrice)
            .GreaterThan(0)
            .WithMessage("Item unit price must be greater than zero.");
    }
}
