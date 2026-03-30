using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;
using FluentAssertions;

namespace Ambev.DeveloperEvaluation.Unit.WebApi;

public class ProductRequestValidatorsTests
{
    [Fact(DisplayName = "Create product request validator should reject empty title")]
    public async Task CreateProductRequestValidator_InvalidRequest_ReturnsError()
    {
        var validator = new CreateProductRequestValidator();
        var result = await validator.ValidateAsync(new CreateProductRequest());

        result.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Get product request validator should reject invalid id")]
    public async Task GetProductRequestValidator_InvalidRequest_ReturnsError()
    {
        var validator = new GetProductRequestValidator();
        var result = await validator.ValidateAsync(new GetProductRequest { Id = 0 });

        result.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "List products request validator should reject invalid size")]
    public async Task ListProductsRequestValidator_InvalidRequest_ReturnsError()
    {
        var validator = new ListProductsRequestValidator();
        var result = await validator.ValidateAsync(new ListProductsRequest { _size = 0 });

        result.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "List products by category request validator should reject empty category")]
    public async Task ListProductsByCategoryRequestValidator_InvalidRequest_ReturnsError()
    {
        var validator = new ListProductsByCategoryRequestValidator();
        var result = await validator.ValidateAsync(new ListProductsByCategoryRequest());

        result.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Update product request validator should reject invalid id")]
    public async Task UpdateProductRequestValidator_InvalidRequest_ReturnsError()
    {
        var validator = new UpdateProductRequestValidator();
        var result = await validator.ValidateAsync(new UpdateProductRequest { Id = 0 });

        result.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Delete product request validator should reject invalid id")]
    public async Task DeleteProductRequestValidator_InvalidRequest_ReturnsError()
    {
        var validator = new DeleteProductRequestValidator();
        var result = await validator.ValidateAsync(new DeleteProductRequest { Id = 0 });

        result.IsValid.Should().BeFalse();
    }
}
