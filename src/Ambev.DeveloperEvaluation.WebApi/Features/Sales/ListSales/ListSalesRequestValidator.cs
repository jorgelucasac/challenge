using FluentValidation;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;

public class ListSalesRequestValidator : AbstractValidator<ListSalesRequest>
{
    public ListSalesRequestValidator()
    {
        RuleFor(request => request._page)
            .GreaterThan(0)
            .When(request => request._page.HasValue);

        RuleFor(request => request._size)
            .GreaterThan(0)
            .LessThanOrEqualTo(ListSalesDefaults.MaxPageSize)
            .When(request => request._size.HasValue);

        RuleFor(request => request._order)
            .Must(ListSalesOrderParser.IsSupported)
            .WithMessage($"Order must be one of: {string.Join(", ", ListSalesOrderParser.SupportedOrders)}")
            .When(request => !string.IsNullOrWhiteSpace(request._order));

        RuleFor(request => request)
            .Must(request => request.SaleDateFrom == null || request.SaleDateTo == null || request.SaleDateFrom <= request.SaleDateTo)
            .WithMessage("SaleDateFrom must be less than or equal to SaleDateTo.");
    }
}
