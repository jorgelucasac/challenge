using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
{
    public UpdateSaleRequestValidator()
    {
        RuleFor(request => request.Id)
            .NotEmpty();

        RuleFor(request => request.SaleDate)
            .NotEmpty();

        RuleFor(request => request.CustomerExternalId)
            .NotEmpty();

        RuleFor(request => request.CustomerName)
            .NotEmpty();

        RuleFor(request => request.BranchExternalId)
            .NotEmpty();

        RuleFor(request => request.BranchName)
            .NotEmpty();

        RuleFor(request => request.Items)
            .NotEmpty();

        RuleForEach(request => request.Items)
            .SetValidator(new UpdateSaleItemRequestValidator());
    }
}

public class UpdateSaleItemRequestValidator : AbstractValidator<UpdateSaleItemRequest>
{
    public UpdateSaleItemRequestValidator()
    {
        RuleFor(item => item)
            .Must(item => !item.IsCancelled || item.Id.HasValue)
            .WithMessage("Cancelled items must have an id.");

        RuleFor(item => item)
            .Must(item => item.Id.HasValue || !item.IsCancelled)
            .WithMessage("New items cannot be cancelled.");

        When(item => !item.IsCancelled, () =>
        {
            RuleFor(item => item.ProductExternalId)
                .NotEmpty();

            RuleFor(item => item.ProductName)
                .NotEmpty();

            RuleFor(item => item.Quantity)
                .GreaterThan(0);

            RuleFor(item => item.UnitPrice)
                .GreaterThan(0);
        });
    }
}
