using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

public class ListSalesValidator : AbstractValidator<ListSalesCommand>
{
    public ListSalesValidator()
    {
        RuleFor(command => command.Page)
            .GreaterThan(0);

        RuleFor(command => command.Size)
            .GreaterThan(0)
            .LessThanOrEqualTo(ListSalesDefaults.MaxPageSize);

        RuleFor(command => command.Order)
            .Must(ListSalesOrderParser.IsSupported)
            .WithMessage($"Order must be one of: {string.Join(", ", ListSalesOrderParser.SupportedOrders)}");

        RuleFor(command => command)
            .Must(command => command.SaleDateFrom == null || command.SaleDateTo == null || command.SaleDateFrom <= command.SaleDateTo)
            .WithMessage("SaleDateFrom must be less than or equal to SaleDateTo.");
    }
}
