using FluentAssertions;
using Xunit;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.Unit.WebApi;

public class GetSaleRequestValidatorTests
{
    private readonly GetSaleRequestValidator _validator = new();

    [Fact(DisplayName = "Request validator should fail for empty id")]
    public void Given_EmptyId_When_Validated_Then_ShouldBeInvalid()
    {
        var result = _validator.Validate(new GetSaleRequest { Id = Guid.Empty });

        result.IsValid.Should().BeFalse();
    }
}
