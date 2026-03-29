using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleValidator : AbstractValidator<UpdateSaleCommand>
{
    public UpdateSaleValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();

        RuleFor(command => command.SaleDate)
            .NotEmpty();

        RuleFor(command => command.CustomerExternalId)
            .NotEmpty();

        RuleFor(command => command.CustomerName)
            .NotEmpty();

        RuleFor(command => command.BranchExternalId)
            .NotEmpty();

        RuleFor(command => command.BranchName)
            .NotEmpty();

        RuleFor(command => command.Items)
            .NotEmpty();

        RuleForEach(command => command.Items)
            .SetValidator(new UpdateSaleItemCommandValidator());
    }
}

public class UpdateSaleItemCommandValidator : AbstractValidator<UpdateSaleItemCommand>
{
    public UpdateSaleItemCommandValidator()
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
