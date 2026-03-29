using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ActivateSale;

public class ActivateSaleRequestValidator : AbstractValidator<ActivateSaleRequest>
{
    public ActivateSaleRequestValidator()
    {
        RuleFor(request => request.Id)
            .NotEmpty();
    }
}
