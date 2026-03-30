using Ambev.DeveloperEvaluation.Application.Products.Common;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;

public class ListProductsRequestValidator : AbstractValidator<ListProductsRequest>
{
    public ListProductsRequestValidator()
    {
        RuleFor(request => request._page)
            .GreaterThan(0)
            .When(request => request._page.HasValue);

        RuleFor(request => request._size)
            .GreaterThan(0)
            .LessThanOrEqualTo(ListProductsDefaults.MaxPageSize)
            .When(request => request._size.HasValue);

        RuleFor(request => request._order)
            .Must(ListProductsOrderParser.IsSupported)
            .WithMessage("Order must use supported fields and directions.")
            .When(request => !string.IsNullOrWhiteSpace(request._order));

        RuleFor(request => request)
            .Must(request => request._minPrice == null || request._maxPrice == null || request._minPrice <= request._maxPrice)
            .WithMessage("_minPrice must be less than or equal to _maxPrice.");
    }
}
