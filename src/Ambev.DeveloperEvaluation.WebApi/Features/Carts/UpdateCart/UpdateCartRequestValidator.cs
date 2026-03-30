using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;

public class UpdateCartRequestValidator : AbstractValidator<UpdateCartRequest>
{
    public UpdateCartRequestValidator()
    {
        RuleFor(cart => cart.Id).GreaterThan(0);
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
