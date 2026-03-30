using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using FluentAssertions;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class CartTests
{
    [Fact]
    public void Create_ValidData_CreatesCartAndAddsEvent()
    {
        var cart = Cart.Create(
            1,
            new DateTime(2026, 3, 30, 0, 0, 0, DateTimeKind.Utc),
            [new CartItemInput(10, 2), new CartItemInput(11, 1)]);

        cart.UserId.Should().Be(1);
        cart.Products.Should().HaveCount(2);
        cart.DomainEvents.Should().ContainSingle().Which.Should().BeOfType<CartCreatedEvent>();
    }

    [Fact]
    public void Update_ValidData_ReplacesProductsAndAddsModifiedEvent()
    {
        var cart = Cart.Create(1, DateTime.UtcNow, [new CartItemInput(10, 2)]);

        cart.Update(2, DateTime.UtcNow.AddDays(1), [new CartItemInput(12, 5)]);

        cart.UserId.Should().Be(2);
        cart.Products.Should().ContainSingle();
        cart.Products.Single().ProductId.Should().Be(12);
        cart.UpdatedAt.Should().NotBeNull();
        cart.DomainEvents.Should().Contain(item => item is CartModifiedEvent);
    }

    [Fact]
    public void Create_WithoutProducts_ThrowsDomainException()
    {
        var act = () => Cart.Create(1, DateTime.UtcNow, []);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void MarkAsDeleted_AddsDeletedEvent()
    {
        var cart = Cart.Create(1, DateTime.UtcNow, [new CartItemInput(10, 2)]);
        SetCartId(cart, 7);

        cart.MarkAsDeleted();

        cart.DomainEvents
            .OfType<CartDeletedEvent>()
            .Should()
            .ContainSingle(item => item.CartId == 7);
    }

    private static void SetCartId(Cart cart, int id)
    {
        typeof(Cart).GetProperty(nameof(Cart.Id))!.SetValue(cart, id);
    }
}
