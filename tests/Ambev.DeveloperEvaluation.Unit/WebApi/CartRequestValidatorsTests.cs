using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.DeleteCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.ListCarts;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;
using FluentAssertions;

namespace Ambev.DeveloperEvaluation.Unit.WebApi;

public class CartRequestValidatorsTests
{
    [Fact]
    public async Task CreateCartRequestValidator_ShouldRejectMissingProducts()
    {
        var validator = new CreateCartRequestValidator();
        var result = await validator.ValidateAsync(new CreateCartRequest { UserId = 1, Date = DateTime.UtcNow });

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task GetCartRequestValidator_ShouldRejectInvalidId()
    {
        var validator = new GetCartRequestValidator();
        var result = await validator.ValidateAsync(new GetCartRequest { Id = 0 });

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task ListCartsRequestValidator_ShouldRejectInvalidPage()
    {
        var validator = new ListCartsRequestValidator();
        var result = await validator.ValidateAsync(new ListCartsRequest { _page = 0 });

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateCartRequestValidator_ShouldRejectInvalidId()
    {
        var validator = new UpdateCartRequestValidator();
        var result = await validator.ValidateAsync(new UpdateCartRequest
        {
            UserId = 1,
            Date = DateTime.UtcNow,
            Products = [new UpdateCartProductRequest { ProductId = 1, Quantity = 1 }]
        });

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteCartRequestValidator_ShouldRejectInvalidId()
    {
        var validator = new DeleteCartRequestValidator();
        var result = await validator.ValidateAsync(new DeleteCartRequest { Id = 0 });

        result.IsValid.Should().BeFalse();
    }
}
