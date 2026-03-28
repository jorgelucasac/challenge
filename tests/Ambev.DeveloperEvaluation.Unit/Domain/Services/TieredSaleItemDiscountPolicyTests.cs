using Ambev.DeveloperEvaluation.Domain.Services;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Services;

public class TieredSaleItemDiscountPolicyTests
{
    private readonly TieredSaleItemDiscountPolicy _policy = new();

    [Theory(DisplayName = "Discount tiers should be resolved by quantity")]
    [InlineData(1, 0)]
    [InlineData(3, 0)]
    [InlineData(4, 10)]
    [InlineData(9, 10)]
    [InlineData(10, 20)]
    [InlineData(20, 20)]
    public void Given_Quantity_When_Calculating_Then_ShouldApplyExpectedDiscount(int quantity, decimal expectedDiscountPercent)
    {
        var pricing = _policy.Calculate(quantity, 10m);

        pricing.DiscountPercent.Should().Be(expectedDiscountPercent);
    }

    [Fact(DisplayName = "Quantities above the supported limit should fail")]
    public void Given_QuantityAboveLimit_When_Calculating_Then_ShouldThrowDomainException()
    {
        var act = () => _policy.Calculate(21, 10m);

        act.Should().Throw<DomainException>()
            .WithMessage("It is not possible to sell more than 20 identical items.");
    }

    [Fact(DisplayName = "Non positive price should fail")]
    public void Given_InvalidPrice_When_Calculating_Then_ShouldThrowDomainException()
    {
        var act = () => _policy.Calculate(1, 0m);

        act.Should().Throw<DomainException>()
            .WithMessage("Item unit price must be greater than zero.");
    }
}
