using FluentAssertions;
using Xunit;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ActivateSale;

namespace Ambev.DeveloperEvaluation.Unit.WebApi;

public class ActivateSaleRequestValidatorTests
{
    private readonly ActivateSaleRequestValidator _validator = new();

    [Fact(DisplayName = "Validator should fail when id is empty")]
    public void Given_EmptyId_When_Validated_Then_ShouldBeInvalid()
    {
        var result = _validator.Validate(new ActivateSaleRequest { Id = Guid.Empty });

        result.IsValid.Should().BeFalse();
    }
}
