using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(product => product.Title)
            .NotEmpty()
            .WithMessage("Product title is required.");

        RuleFor(product => product.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Product price must be greater than or equal to zero.");

        RuleFor(product => product.Description)
            .NotEmpty()
            .WithMessage("Product description is required.");

        RuleFor(product => product.Category)
            .NotEmpty()
            .WithMessage("Product category is required.");

        RuleFor(product => product.Image)
            .NotEmpty()
            .WithMessage("Product image is required.");

        RuleFor(product => product.Rating)
            .NotNull()
            .WithMessage("Product rating is required.");
    }
}
