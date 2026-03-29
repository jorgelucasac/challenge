using FluentAssertions;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Unit.Domain;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CreateSaleCommandValidatorTests
{
    private readonly CreateSaleCommandValidator _validator = new();

    [Fact(DisplayName = "Validator should fail when sale has no items")]
    public void Given_CommandWithoutItems_When_Validated_Then_ShouldBeInvalid()
    {
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        command.Items = [];

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Validator should fail when item has invalid quantity")]
    public void Given_CommandWithInvalidItem_When_Validated_Then_ShouldBeInvalid()
    {
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        command.Items[0].Quantity = 0;

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }
}
