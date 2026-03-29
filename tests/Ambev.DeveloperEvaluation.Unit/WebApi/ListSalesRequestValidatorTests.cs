using FluentAssertions;
using Xunit;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;

namespace Ambev.DeveloperEvaluation.Unit.WebApi;

public class ListSalesRequestValidatorTests
{
    private readonly ListSalesRequestValidator _validator = new();

    [Fact(DisplayName = "Validator should fail when page is invalid")]
    public void Given_InvalidPage_When_Validated_Then_ShouldBeInvalid()
    {
        var request = new ListSalesRequest { _page = 0 };

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Validator should fail when size is above limit")]
    public void Given_InvalidPageSize_When_Validated_Then_ShouldBeInvalid()
    {
        var request = new ListSalesRequest { _size = 101 };

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Validator should fail when order is invalid")]
    public void Given_InvalidOrder_When_Validated_Then_ShouldBeInvalid()
    {
        var request = new ListSalesRequest { _order = "createdAt_asc" };

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Validator should fail when date range is invalid")]
    public void Given_InvalidDateRange_When_Validated_Then_ShouldBeInvalid()
    {
        var request = new ListSalesRequest
        {
            SaleDateFrom = new DateTime(2026, 3, 30),
            SaleDateTo = new DateTime(2026, 3, 29)
        };

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Validator should pass when request is valid")]
    public void Given_ValidRequest_When_Validated_Then_ShouldBeValid()
    {
        var request = new ListSalesRequest
        {
            _page = 1,
            _size = 10,
            _order = "saleDate_desc"
        };

        var result = _validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }
}
