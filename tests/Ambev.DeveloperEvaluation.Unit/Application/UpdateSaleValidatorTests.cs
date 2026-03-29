using FluentAssertions;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class UpdateSaleValidatorTests
{
    private readonly UpdateSaleValidator _validator = new();

    [Fact(DisplayName = "Validator should fail when id is empty")]
    public void Given_EmptyId_When_Validated_Then_ShouldBeInvalid()
    {
        var command = new UpdateSaleCommand { Id = Guid.Empty };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Validator should fail when active sale has no items")]
    public void Given_ActiveSaleWithoutItems_When_Validated_Then_ShouldBeInvalid()
    {
        var command = new UpdateSaleCommand
        {
            Id = Guid.NewGuid(),
            SaleDate = DateTime.UtcNow,
            CustomerExternalId = "customer-1",
            CustomerName = "Customer",
            BranchExternalId = "branch-1",
            BranchName = "Branch"
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Validator should fail when new item is cancelled")]
    public void Given_NewCancelledItem_When_Validated_Then_ShouldBeInvalid()
    {
        var command = new UpdateSaleCommand
        {
            Id = Guid.NewGuid(),
            SaleDate = DateTime.UtcNow,
            CustomerExternalId = "customer-1",
            CustomerName = "Customer",
            BranchExternalId = "branch-1",
            BranchName = "Branch",
            Items = [new UpdateSaleItemCommand { IsCancelled = true }]
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Validator should fail when cancelled item has no id")]
    public void Given_CancelledItemWithoutId_When_Validated_Then_ShouldBeInvalid()
    {
        var command = new UpdateSaleCommand
        {
            Id = Guid.NewGuid(),
            SaleDate = DateTime.UtcNow,
            CustomerExternalId = "customer-1",
            CustomerName = "Customer",
            BranchExternalId = "branch-1",
            BranchName = "Branch",
            Items = [new UpdateSaleItemCommand { IsCancelled = true }]
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Validator should fail when active item has invalid values")]
    public void Given_InvalidActiveItem_When_Validated_Then_ShouldBeInvalid()
    {
        var command = new UpdateSaleCommand
        {
            Id = Guid.NewGuid(),
            SaleDate = DateTime.UtcNow,
            CustomerExternalId = "customer-1",
            CustomerName = "Customer",
            BranchExternalId = "branch-1",
            BranchName = "Branch",
            Items = [new UpdateSaleItemCommand { Id = Guid.NewGuid(), ProductExternalId = "product-1", ProductName = "Product", Quantity = 0, UnitPrice = 10m }]
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Validator should pass when request is valid")]
    public void Given_ValidRequest_When_Validated_Then_ShouldBeValid()
    {
        var command = new UpdateSaleCommand
        {
            Id = Guid.NewGuid(),
            SaleDate = DateTime.UtcNow,
            CustomerExternalId = "customer-1",
            CustomerName = "Customer",
            BranchExternalId = "branch-1",
            BranchName = "Branch",
            Items = [new UpdateSaleItemCommand { Id = Guid.NewGuid(), ProductExternalId = "product-1", ProductName = "Product", Quantity = 1, UnitPrice = 10m }]
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }
}
