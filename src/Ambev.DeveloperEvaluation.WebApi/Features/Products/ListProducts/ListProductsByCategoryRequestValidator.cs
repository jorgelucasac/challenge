using FluentValidation;
using Ambev.DeveloperEvaluation.Application.Products.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;

public class ListProductsByCategoryRequestValidator : AbstractValidator<ListProductsByCategoryRequest>
{
    public ListProductsByCategoryRequestValidator()
    {
        RuleFor(request => request.Category)
            .NotEmpty();

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
    }
}
