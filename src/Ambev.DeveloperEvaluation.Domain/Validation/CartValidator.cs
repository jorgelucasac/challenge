using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class CartValidator : AbstractValidator<Cart>
{
    public CartValidator()
    {
        RuleFor(cart => cart.UserId)
            .GreaterThan(0);

        RuleFor(cart => cart.Date)
            .NotEmpty();

        RuleFor(cart => cart.Products)
            .NotEmpty();

        RuleForEach(cart => cart.Products)
            .ChildRules(item =>
            {
                item.RuleFor(product => product.ProductId)
                    .GreaterThan(0);

                item.RuleFor(product => product.Quantity)
                    .GreaterThan(0);
            });
    }
}
