using FluentAssertions;
using Xunit;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

namespace Ambev.DeveloperEvaluation.Unit.WebApi;

public class UpdateSaleRequestValidatorTests
{
    private readonly UpdateSaleRequestValidator _validator = new();

    [Fact(DisplayName = "Validator should fail when route id is empty")]
    public void Given_EmptyId_When_Validated_Then_ShouldBeInvalid()
    {
        var request = new UpdateSaleRequest { Id = Guid.Empty };

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Validator should fail when active request has no items")]
    public void Given_ActiveRequestWithoutItems_When_Validated_Then_ShouldBeInvalid()
    {
        var request = new UpdateSaleRequest
        {
            Id = Guid.NewGuid(),
            SaleDate = DateTime.UtcNow,
            CustomerExternalId = "customer-1",
            CustomerName = "Customer",
            BranchExternalId = "branch-1",
            BranchName = "Branch"
        };

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Validator should fail when cancelled item has no id")]
    public void Given_CancelledItemWithoutId_When_Validated_Then_ShouldBeInvalid()
    {
        var request = new UpdateSaleRequest
        {
            Id = Guid.NewGuid(),
            SaleDate = DateTime.UtcNow,
            CustomerExternalId = "customer-1",
            CustomerName = "Customer",
            BranchExternalId = "branch-1",
            BranchName = "Branch",
            Items = [new UpdateSaleItemRequest { IsCancelled = true }]
        };

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Validator should pass when request is valid")]
    public void Given_ValidRequest_When_Validated_Then_ShouldBeValid()
    {
        var request = new UpdateSaleRequest
        {
            Id = Guid.NewGuid(),
            SaleDate = DateTime.UtcNow,
            CustomerExternalId = "customer-1",
            CustomerName = "Customer",
            BranchExternalId = "branch-1",
            BranchName = "Branch",
            Items =
            [
                new UpdateSaleItemRequest
                {
                    Id = Guid.NewGuid(),
                    ProductExternalId = "product-1",
                    ProductName = "Product",
                    Quantity = 1,
                    UnitPrice = 10m
                }
            ]
        };

        var result = _validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }
}
