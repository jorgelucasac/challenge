using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using FluentAssertions;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class ProductTests
{
    [Fact(DisplayName = "Given valid product data When creating product Then creates aggregate")]
    public void Create_ValidData_CreatesProduct()
    {
        var product = Product.Create(
            "Backpack",
            109.95m,
            "Travel backpack",
            "bags",
            "https://image.test/backpack.png",
            4.5m,
            12);

        product.Title.Should().Be("Backpack");
        product.Price.Should().Be(109.95m);
        product.Rating.Rate.Should().Be(4.5m);
        product.Rating.Count.Should().Be(12);
        product.DomainEvents.Should().ContainSingle().Which.Should().BeOfType<ProductCreatedEvent>();
    }

    [Fact(DisplayName = "Given valid product When updating Then refreshes data and timestamp")]
    public void Update_ValidData_UpdatesProduct()
    {
        var product = Product.Create(
            "Backpack",
            109.95m,
            "Travel backpack",
            "bags",
            "https://image.test/backpack.png",
            4.5m,
            12);

        product.Update(
            "Jacket",
            89.90m,
            "Winter jacket",
            "clothing",
            "https://image.test/jacket.png",
            4.9m,
            30);

        product.Title.Should().Be("Jacket");
        product.Category.Should().Be("clothing");
        product.UpdatedAt.Should().NotBeNull();
        product.DomainEvents.Should().Contain(item => item is ProductModifiedEvent);
    }

    [Fact(DisplayName = "Given negative rating count When creating Then throws domain exception")]
    public void Create_InvalidRating_ThrowsDomainException()
    {
        var act = () => Product.Create(
            "Backpack",
            109.95m,
            "Travel backpack",
            "bags",
            "https://image.test/backpack.png",
            4.5m,
            -1);

        act.Should().Throw<DomainException>();
    }

    [Fact(DisplayName = "Given product When marking as deleted Then adds deleted event")]
    public void MarkAsDeleted_Product_AddsDeletedEvent()
    {
        var product = Product.Create(
            "Backpack",
            109.95m,
            "Travel backpack",
            "bags",
            "https://image.test/backpack.png",
            4.5m,
            12);

        SetProductId(product, 42);
        product.MarkAsDeleted();

        product.DomainEvents
            .OfType<ProductDeletedEvent>()
            .Should()
            .ContainSingle(item => item.ProductId == 42);
    }

    private static void SetProductId(Product product, int id)
    {
        typeof(Product)
            .GetProperty(nameof(Product.Id))!
            .SetValue(product, id);
    }
}
