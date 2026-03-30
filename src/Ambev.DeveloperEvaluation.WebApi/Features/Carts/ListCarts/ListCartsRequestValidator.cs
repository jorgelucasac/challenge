using Ambev.DeveloperEvaluation.Application.Carts.Common;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.ListCarts;

public class ListCartsRequestValidator : AbstractValidator<ListCartsRequest>
{
    public ListCartsRequestValidator()
    {
        RuleFor(cart => cart._page).GreaterThan(0);
        RuleFor(cart => cart._size).GreaterThan(0).LessThanOrEqualTo(ListCartsDefaults.MaxPageSize);
        RuleFor(cart => cart._order)
            .Must(ListCartsOrderParser.IsSupported)
            .WithMessage("Order must use supported fields and directions.");
    }
}
