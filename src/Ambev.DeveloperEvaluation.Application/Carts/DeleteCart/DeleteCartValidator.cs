using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;

public class DeleteCartValidator : AbstractValidator<DeleteCartCommand>
{
    public DeleteCartValidator()
    {
        RuleFor(cart => cart.Id).GreaterThan(0);
    }
}
