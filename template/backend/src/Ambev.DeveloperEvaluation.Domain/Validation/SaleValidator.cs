using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class SaleValidator : AbstractValidator<Sale>
{
    public SaleValidator()
    {
        RuleFor(sale => sale.SaleNumber)
            .NotEmpty()
            .WithMessage("Sale number is required.");

        RuleFor(sale => sale.CustomerExternalId)
            .NotEmpty()
            .WithMessage("Customer external id is required.");

        RuleFor(sale => sale.CustomerName)
            .NotEmpty()
            .WithMessage("Customer name is required.");

        RuleFor(sale => sale.BranchExternalId)
            .NotEmpty()
            .WithMessage("Branch external id is required.");

        RuleFor(sale => sale.BranchName)
            .NotEmpty()
            .WithMessage("Branch name is required.");

        RuleFor(sale => sale.Items)
            .NotEmpty()
            .WithMessage("A sale must have at least one item.");

        RuleForEach(sale => sale.Items)
            .SetValidator(new SaleItemValidator());
    }
}
