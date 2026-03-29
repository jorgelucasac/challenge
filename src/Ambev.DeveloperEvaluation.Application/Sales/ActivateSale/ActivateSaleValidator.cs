using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.ActivateSale;

public class ActivateSaleValidator : AbstractValidator<ActivateSaleCommand>
{
    public ActivateSaleValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();
    }
}
