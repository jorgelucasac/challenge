using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(command => command.Id)
            .GreaterThan(0);

        RuleFor(command => command.Title)
            .NotEmpty();

        RuleFor(command => command.Price)
            .GreaterThanOrEqualTo(0);

        RuleFor(command => command.Description)
            .NotEmpty();

        RuleFor(command => command.Category)
            .NotEmpty();

        RuleFor(command => command.Image)
            .NotEmpty();

        RuleFor(command => command.RatingRate)
            .GreaterThanOrEqualTo(0);

        RuleFor(command => command.RatingCount)
            .GreaterThanOrEqualTo(0);
    }
}
