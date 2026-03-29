using FluentAssertions;
using Xunit;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

namespace Ambev.DeveloperEvaluation.Unit.WebApi;

public class CancelSaleRequestValidatorTests
{
    private readonly CancelSaleRequestValidator _validator = new();

    [Fact(DisplayName = "Validator should fail when id is empty")]
    public void Given_EmptyId_When_Validated_Then_ShouldBeInvalid()
    {
        var result = _validator.Validate(new CancelSaleRequest { Id = Guid.Empty });

        result.IsValid.Should().BeFalse();
    }
}
