using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
{
    public CreateSaleRequestValidator()
    {
        RuleFor(request => request.SaleDate).NotEmpty();
        RuleFor(request => request.CustomerExternalId).NotEmpty();
        RuleFor(request => request.CustomerName).NotEmpty();
        RuleFor(request => request.BranchExternalId).NotEmpty();
        RuleFor(request => request.BranchName).NotEmpty();
        RuleFor(request => request.Items).NotEmpty();
        RuleForEach(request => request.Items).SetValidator(new CreateSaleItemRequestValidator());
    }
}

public class CreateSaleItemRequestValidator : AbstractValidator<CreateSaleItemRequest>
{
    public CreateSaleItemRequestValidator()
    {
        RuleFor(item => item.ProductExternalId).NotEmpty();
        RuleFor(item => item.ProductName).NotEmpty();
        RuleFor(item => item.Quantity).GreaterThan(0);
        RuleFor(item => item.UnitPrice).GreaterThan(0);
    }
}
