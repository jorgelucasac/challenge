using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;
using Ambev.DeveloperEvaluation.Application.Carts.GetCart;
using Ambev.DeveloperEvaluation.Application.Carts.ListCarts;
using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using FluentAssertions;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CartValidatorsTests
{
    [Fact]
    public void CreateCartValidator_ShouldRejectMissingProducts()
    {
        var validator = new CreateCartValidator();
        var result = validator.Validate(new CreateCartCommand { UserId = 1, Date = DateTime.UtcNow });

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void GetCartValidator_ShouldRejectInvalidId()
    {
        var validator = new GetCartValidator();
        var result = validator.Validate(new GetCartCommand(0));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void ListCartsValidator_ShouldRejectInvalidOrder()
    {
        var validator = new ListCartsValidator();
        var result = validator.Validate(new ListCartsCommand { Order = "createdAt desc" });

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void UpdateCartValidator_ShouldRejectInvalidId()
    {
        var validator = new UpdateCartValidator();
        var result = validator.Validate(new UpdateCartCommand
        {
            UserId = 1,
            Date = DateTime.UtcNow,
            Products = [new UpdateCartProductInput { ProductId = 1, Quantity = 1 }]
        });

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void DeleteCartValidator_ShouldRejectInvalidId()
    {
        var validator = new DeleteCartValidator();
        var result = validator.Validate(new DeleteCartCommand(0));

        result.IsValid.Should().BeFalse();
    }
}
