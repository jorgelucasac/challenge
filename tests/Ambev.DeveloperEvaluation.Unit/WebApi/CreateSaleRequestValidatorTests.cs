using FluentAssertions;
using Xunit;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.Unit.WebApi;

public class CreateSaleRequestValidatorTests
{
    private readonly CreateSaleRequestValidator _validator = new();

    [Fact(DisplayName = "Request validator should fail when customer snapshot is missing")]
    public void Given_RequestWithoutCustomer_When_Validated_Then_ShouldBeInvalid()
    {
        var request = new CreateSaleRequest
        {
            SaleDate = DateTime.UtcNow,
            BranchExternalId = "branch-1",
            BranchName = "Branch",
            Items = [new CreateSaleItemRequest { ProductExternalId = "product-1", ProductName = "Product", Quantity = 1, UnitPrice = 10m }]
        };

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Request validator should fail when item price is invalid")]
    public void Given_RequestWithInvalidPrice_When_Validated_Then_ShouldBeInvalid()
    {
        var request = new CreateSaleRequest
        {
            SaleDate = DateTime.UtcNow,
            CustomerExternalId = "customer-1",
            CustomerName = "Customer",
            BranchExternalId = "branch-1",
            BranchName = "Branch",
            Items = [new CreateSaleItemRequest { ProductExternalId = "product-1", ProductName = "Product", Quantity = 1, UnitPrice = 0m }]
        };

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Request validator should fail when sale date is missing")]
    public void Given_RequestWithoutSaleDate_When_Validated_Then_ShouldBeInvalid()
    {
        var request = new CreateSaleRequest
        {
            CustomerExternalId = "customer-1",
            CustomerName = "Customer",
            BranchExternalId = "branch-1",
            BranchName = "Branch",
            Items = [new CreateSaleItemRequest { ProductExternalId = "product-1", ProductName = "Product", Quantity = 1, UnitPrice = 10m }]
        };

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }
}
