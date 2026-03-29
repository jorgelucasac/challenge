using FluentAssertions;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class GetSaleValidatorTests
{
    private readonly GetSaleValidator _validator = new();

    [Fact(DisplayName = "Validator should fail for empty id")]
    public void Given_EmptyId_When_Validated_Then_ShouldBeInvalid()
    {
        var result = _validator.Validate(new GetSaleCommand(Guid.Empty));

        result.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Validator should pass for valid id")]
    public void Given_ValidId_When_Validated_Then_ShouldBeValid()
    {
        var result = _validator.Validate(new GetSaleCommand(Guid.NewGuid()));

        result.IsValid.Should().BeTrue();
    }
}
