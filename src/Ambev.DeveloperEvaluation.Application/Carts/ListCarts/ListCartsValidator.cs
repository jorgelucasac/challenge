using Ambev.DeveloperEvaluation.Application.Carts.Common;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.ListCarts;

public class ListCartsValidator : AbstractValidator<ListCartsCommand>
{
    public ListCartsValidator()
    {
        RuleFor(cart => cart.Page).GreaterThan(0);
        RuleFor(cart => cart.Size).GreaterThan(0).LessThanOrEqualTo(ListCartsDefaults.MaxPageSize);
        RuleFor(cart => cart.Order)
            .Must(ListCartsOrderParser.IsSupported)
            .WithMessage("Order must use supported fields and directions.");
    }
}
