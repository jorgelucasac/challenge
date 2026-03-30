using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.ListProducts;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using FluentAssertions;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class ProductValidatorsTests
{
    [Fact(DisplayName = "Create product validator should reject empty title")]
    public void CreateProductValidator_InvalidTitle_ReturnsError()
    {
        var validator = new CreateProductValidator();
        var result = validator.Validate(new CreateProductCommand());

        result.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Get product validator should reject non-positive id")]
    public void GetProductValidator_InvalidId_ReturnsError()
    {
        var validator = new GetProductValidator();
        var result = validator.Validate(new GetProductCommand(0));

        result.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "List products validator should reject invalid range")]
    public void ListProductsValidator_InvalidRange_ReturnsError()
    {
        var validator = new ListProductsValidator();
        var result = validator.Validate(new ListProductsCommand { MinPrice = 10, MaxPrice = 5 });

        result.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Update product validator should reject invalid id")]
    public void UpdateProductValidator_InvalidId_ReturnsError()
    {
        var validator = new UpdateProductValidator();
        var result = validator.Validate(new UpdateProductCommand { Id = 0 });

        result.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Delete product validator should reject invalid id")]
    public void DeleteProductValidator_InvalidId_ReturnsError()
    {
        var validator = new DeleteProductValidator();
        var result = validator.Validate(new DeleteProductCommand(0));

        result.IsValid.Should().BeFalse();
    }
}
