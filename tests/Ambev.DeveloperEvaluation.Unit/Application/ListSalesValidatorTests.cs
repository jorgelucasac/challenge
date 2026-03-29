using FluentAssertions;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class ListSalesValidatorTests
{
    private readonly ListSalesValidator _validator = new();

    [Fact(DisplayName = "Validator should fail when page is invalid")]
    public void Given_InvalidPage_When_Validated_Then_ShouldBeInvalid()
    {
        var command = new ListSalesCommand { Page = 0 };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Validator should fail when size is above limit")]
    public void Given_PageSizeAboveLimit_When_Validated_Then_ShouldBeInvalid()
    {
        var command = new ListSalesCommand { Size = ListSalesDefaults.MaxPageSize + 1 };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Validator should fail when order is unsupported")]
    public void Given_UnsupportedOrder_When_Validated_Then_ShouldBeInvalid()
    {
        var command = new ListSalesCommand { Order = "createdAt_desc" };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Validator should fail when date range is invalid")]
    public void Given_InvalidDateRange_When_Validated_Then_ShouldBeInvalid()
    {
        var command = new ListSalesCommand
        {
            SaleDateFrom = new DateTime(2026, 3, 30),
            SaleDateTo = new DateTime(2026, 3, 29)
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Validator should pass when command is valid")]
    public void Given_ValidCommand_When_Validated_Then_ShouldBeValid()
    {
        var command = new ListSalesCommand
        {
            Page = 1,
            Size = 10,
            Order = "saleDate_desc",
            SaleDateFrom = new DateTime(2026, 3, 29),
            SaleDateTo = new DateTime(2026, 3, 30)
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }
}
