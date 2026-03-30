using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

public class CreateCartValidator : AbstractValidator<CreateCartCommand>
{
    public CreateCartValidator()
    {
        RuleFor(cart => cart.UserId).GreaterThan(0);
        RuleFor(cart => cart.Date).NotEmpty();
        RuleFor(cart => cart.Products).NotEmpty();
        RuleForEach(cart => cart.Products).ChildRules(product =>
        {
            product.RuleFor(item => item.ProductId).GreaterThan(0);
            product.RuleFor(item => item.Quantity).GreaterThan(0);
        });
    }
}
