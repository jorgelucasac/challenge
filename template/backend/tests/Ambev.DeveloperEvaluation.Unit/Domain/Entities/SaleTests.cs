using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleTests
{
    [Fact(DisplayName = "Item with up to three units should not receive discount")]
    public void Given_ItemWithThreeUnits_When_Created_Then_ShouldNotApplyDiscount()
    {
        var sale = Sale.Create(
            "SALE-100",
            DateTime.UtcNow,
            "customer-1",
            "Customer",
            "branch-1",
            "Branch",
            [new SaleItemInput("product-1", "Product", 3, 10m)]);

        var item = sale.Items.Single();

        item.DiscountPercent.Should().Be(0m);
        item.DiscountAmount.Should().Be(0m);
        item.TotalAmount.Should().Be(30m);
        sale.TotalAmount.Should().Be(30m);
    }

    [Fact(DisplayName = "Item with four to nine units should receive ten percent discount")]
    public void Given_ItemWithFourUnits_When_Created_Then_ShouldApplyTenPercentDiscount()
    {
        var sale = Sale.Create(
            "SALE-101",
            DateTime.UtcNow,
            "customer-1",
            "Customer",
            "branch-1",
            "Branch",
            [new SaleItemInput("product-1", "Product", 4, 10m)]);

        var item = sale.Items.Single();

        item.DiscountPercent.Should().Be(10m);
        item.DiscountAmount.Should().Be(4m);
        item.TotalAmount.Should().Be(36m);
        sale.TotalAmount.Should().Be(36m);
    }

    [Fact(DisplayName = "Item with ten to twenty units should receive twenty percent discount")]
    public void Given_ItemWithTenUnits_When_Created_Then_ShouldApplyTwentyPercentDiscount()
    {
        var sale = Sale.Create(
            "SALE-102",
            DateTime.UtcNow,
            "customer-1",
            "Customer",
            "branch-1",
            "Branch",
            [new SaleItemInput("product-1", "Product", 10, 10m)]);

        var item = sale.Items.Single();

        item.DiscountPercent.Should().Be(20m);
        item.DiscountAmount.Should().Be(20m);
        item.TotalAmount.Should().Be(80m);
        sale.TotalAmount.Should().Be(80m);
    }

    [Fact(DisplayName = "Item with more than twenty units should fail")]
    public void Given_ItemWithTwentyOneUnits_When_Created_Then_ShouldThrowDomainException()
    {
        var act = () => Sale.Create(
            "SALE-103",
            DateTime.UtcNow,
            "customer-1",
            "Customer",
            "branch-1",
            "Branch",
            [new SaleItemInput("product-1", "Product", 21, 10m)]);

        act.Should().Throw<DomainException>()
            .WithMessage("It is not possible to sell more than 20 identical items.");
    }

    [Fact(DisplayName = "Sale total should be the sum of non cancelled items")]
    public void Given_MultipleItems_When_Created_Then_ShouldCalculateSaleTotal()
    {
        var sale = Sale.Create(
            "SALE-104",
            DateTime.UtcNow,
            "customer-1",
            "Customer",
            "branch-1",
            "Branch",
            [
                new SaleItemInput("product-1", "Product One", 2, 10m),
                new SaleItemInput("product-2", "Product Two", 4, 10m)
            ]);

        sale.TotalAmount.Should().Be(56m);
    }

    [Fact(DisplayName = "Cancelling an item should recalculate sale total")]
    public void Given_ExistingSale_When_ItemCancelled_Then_ShouldRecalculateTotal()
    {
        var sale = Sale.Create(
            "SALE-105",
            DateTime.UtcNow,
            "customer-1",
            "Customer",
            "branch-1",
            "Branch",
            [
                new SaleItemInput("product-1", "Product One", 2, 10m),
                new SaleItemInput("product-2", "Product Two", 4, 10m)
            ]);

        var itemToCancel = sale.Items.Last();

        sale.CancelItem(itemToCancel.Id);

        itemToCancel.IsCancelled.Should().BeTrue();
        itemToCancel.TotalAmount.Should().Be(0m);
        sale.TotalAmount.Should().Be(20m);
    }

    [Fact(DisplayName = "Cancelling a sale should cancel items and zero total")]
    public void Given_ExistingSale_When_Cancelled_Then_ShouldZeroTotal()
    {
        var sale = SaleTestData.GenerateValidSale();

        sale.Cancel();

        sale.IsCancelled.Should().BeTrue();
        sale.TotalAmount.Should().Be(0m);
        sale.Items.Should().OnlyContain(item => item.IsCancelled);
    }

    [Fact(DisplayName = "Item with invalid quantity should fail")]
    public void Given_ItemWithInvalidQuantity_When_Created_Then_ShouldThrowDomainException()
    {
        var act = () => Sale.Create(
            "SALE-106",
            DateTime.UtcNow,
            "customer-1",
            "Customer",
            "branch-1",
            "Branch",
            [new SaleItemInput("product-1", "Product", 0, 10m)]);

        act.Should().Throw<DomainException>()
            .WithMessage("Item quantity must be greater than zero.");
    }

    [Fact(DisplayName = "Item with invalid unit price should fail")]
    public void Given_ItemWithInvalidPrice_When_Created_Then_ShouldThrowDomainException()
    {
        var act = () => Sale.Create(
            "SALE-107",
            DateTime.UtcNow,
            "customer-1",
            "Customer",
            "branch-1",
            "Branch",
            [new SaleItemInput("product-1", "Product", 1, 0m)]);

        act.Should().Throw<DomainException>()
            .WithMessage("Item unit price must be greater than zero.");
    }

    [Fact(DisplayName = "Sale without items should fail")]
    public void Given_SaleWithoutItems_When_Created_Then_ShouldThrowDomainException()
    {
        var act = () => Sale.Create(
            "SALE-108",
            DateTime.UtcNow,
            "customer-1",
            "Customer",
            "branch-1",
            "Branch",
            []);

        act.Should().Throw<DomainException>()
            .WithMessage("A sale must have at least one item.");
    }

    [Fact(DisplayName = "Valid sale data should pass validation")]
    public void Given_ValidSale_When_Validated_Then_ShouldReturnValid()
    {
        var sale = SaleTestData.GenerateValidSale();

        var result = sale.Validate();

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}
