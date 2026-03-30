using Ambev.DeveloperEvaluation.Application.Products.Common;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProducts;

public class ListProductsValidator : AbstractValidator<ListProductsCommand>
{
    public ListProductsValidator()
    {
        RuleFor(command => command.Page)
            .GreaterThan(0);

        RuleFor(command => command.Size)
            .GreaterThan(0)
            .LessThanOrEqualTo(ListProductsDefaults.MaxPageSize);

        RuleFor(command => command.Order)
            .Must(ListProductsOrderParser.IsSupported)
            .WithMessage("Order must use supported fields and directions.");

        RuleFor(command => command)
            .Must(command => command.MinPrice == null || command.MaxPrice == null || command.MinPrice <= command.MaxPrice)
            .WithMessage("MinPrice must be less than or equal to MaxPrice.");
    }
}
