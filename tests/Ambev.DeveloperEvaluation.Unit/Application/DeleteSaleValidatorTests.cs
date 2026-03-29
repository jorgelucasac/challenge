using FluentAssertions;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class DeleteSaleValidatorTests
{
    private readonly DeleteSaleValidator _validator = new();

    [Fact(DisplayName = "Validator should fail when id is empty")]
    public void Given_EmptyId_When_Validated_Then_ShouldBeInvalid()
    {
        var result = _validator.Validate(new DeleteSaleCommand(Guid.Empty));

        result.IsValid.Should().BeFalse();
    }
}
