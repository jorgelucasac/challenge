using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(request => request.Title)
            .NotEmpty();

        RuleFor(request => request.Price)
            .GreaterThanOrEqualTo(0);

        RuleFor(request => request.Description)
            .NotEmpty();

        RuleFor(request => request.Category)
            .NotEmpty();

        RuleFor(request => request.Image)
            .NotEmpty();

        RuleFor(request => request.Rating)
            .NotNull()
            .ChildRules(rating =>
            {
                rating.RuleFor(item => item.Rate)
                    .GreaterThanOrEqualTo(0);

                rating.RuleFor(item => item.Count)
                    .GreaterThanOrEqualTo(0);
            });
    }
}
