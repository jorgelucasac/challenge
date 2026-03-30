using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
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
