using Ambev.DeveloperEvaluation.Application.Products.Common;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.ListProductCategories;
using Ambev.DeveloperEvaluation.Application.Products.ListProducts;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace Ambev.DeveloperEvaluation.Unit.WebApi;

public class ProductsControllerTests
{
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _controller = new ProductsController(_mediator, _mapper);
    }

    [Fact(DisplayName = "Given valid request When creating product Then returns created payload")]
    public async Task CreateProduct_ValidRequest_ReturnsCreatedResult()
    {
        var request = BuildCreateRequest();
        var command = new CreateProductCommand();
        var result = new ProductResult { Id = 1, Title = "Backpack" };
        var response = new ProductResponse { Id = 1, Title = "Backpack" };

        _mapper.Map<CreateProductCommand>(request).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>()).Returns(result);
        _mapper.Map<ProductResponse>(result).Returns(response);

        var actionResult = await _controller.CreateProduct(request, CancellationToken.None);

        var createdResult = actionResult.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.Value.Should().Be(response);
    }

    [Fact(DisplayName = "Given valid id When getting product Then returns ok payload")]
    public async Task GetProduct_ValidId_ReturnsOkResult()
    {
        var result = new ProductResult { Id = 1, Title = "Backpack" };
        var response = new ProductResponse { Id = 1, Title = "Backpack" };

        _mediator.Send(Arg.Any<GetProductCommand>(), Arg.Any<CancellationToken>()).Returns(result);
        _mapper.Map<ProductResponse>(result).Returns(response);

        var actionResult = await _controller.GetProduct(1, CancellationToken.None);

        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().Be(response);
    }

    [Fact(DisplayName = "Given valid filters When listing products Then returns paged payload")]
    public async Task ListProducts_ValidRequest_ReturnsOkResult()
    {
        var request = new ListProductsRequest { _page = 2, _size = 5, _order = "price desc, title asc" };
        var command = new ListProductsCommand();
        var result = new PagedResult<ProductResult>([new ProductResult { Id = 1, Title = "Backpack" }], 2, 5, 8);
        var responseItems = new List<ProductResponse> { new() { Id = 1, Title = "Backpack" } };

        _mapper.Map<ListProductsCommand>(request).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>()).Returns(result);
        _mapper.Map<List<ProductResponse>>(result.Items).Returns(responseItems);

        var actionResult = await _controller.ListProducts(request, CancellationToken.None);

        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        var payload = okResult.Value.Should().BeOfType<PagedProductsResponse>().Subject;
        payload.TotalItems.Should().Be(8);
        payload.Data.Should().ContainSingle();
    }

    [Fact(DisplayName = "Given valid category request When listing products by category Then returns ok payload")]
    public async Task ListProductsByCategory_ValidRequest_ReturnsOkResult()
    {
        var request = new ListProductsByCategoryRequest { _page = 1, _size = 10, _order = "price desc" };
        var command = new ListProductsCommand();
        var result = new PagedResult<ProductResult>([new ProductResult { Id = 1, Title = "Backpack", Category = "bags" }], 1, 10, 1);
        var responseItems = new List<ProductResponse> { new() { Id = 1, Title = "Backpack", Category = "bags" } };

        _mapper.Map<ListProductsCommand>(Arg.Any<ListProductsByCategoryRequest>()).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>()).Returns(result);
        _mapper.Map<List<ProductResponse>>(result.Items).Returns(responseItems);

        var actionResult = await _controller.ListProductsByCategory("bags", request, CancellationToken.None);

        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        var payload = okResult.Value.Should().BeOfType<PagedProductsResponse>().Subject;
        payload.Data.Should().ContainSingle();
    }

    [Fact(DisplayName = "Given categories query When listing categories Then returns ok payload")]
    public async Task ListCategories_ReturnsOkResult()
    {
        _mediator.Send(Arg.Any<ListProductCategoriesQuery>(), Arg.Any<CancellationToken>())
            .Returns(["bags", "clothing"]);

        var actionResult = await _controller.ListCategories(CancellationToken.None);

        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(new[] { "bags", "clothing" });
    }

    [Fact(DisplayName = "Given valid request When updating product Then returns ok payload")]
    public async Task UpdateProduct_ValidRequest_ReturnsOkResult()
    {
        var request = new UpdateProductRequest
        {
            Title = "Jacket",
            Price = 89.90m,
            Description = "Winter jacket",
            Category = "clothing",
            Image = "https://image.test/jacket.png",
            Rating = new UpdateProductRatingRequest { Rate = 4.9m, Count = 20 }
        };
        var command = new UpdateProductCommand();
        var result = new ProductResult { Id = 1, Title = "Jacket" };
        var response = new ProductResponse { Id = 1, Title = "Jacket" };

        _mapper.Map<UpdateProductCommand>(Arg.Any<UpdateProductRequest>()).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>()).Returns(result);
        _mapper.Map<ProductResponse>(result).Returns(response);

        var actionResult = await _controller.UpdateProduct(1, request, CancellationToken.None);

        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().Be(response);
    }

    [Fact(DisplayName = "Given valid id When deleting product Then returns message")]
    public async Task DeleteProduct_ValidId_ReturnsOkResult()
    {
        var actionResult = await _controller.DeleteProduct(1, CancellationToken.None);

        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeOfType<DeleteProductResponse>().Which.Message.Should().Be("Product deleted successfully");
    }

    private static CreateProductRequest BuildCreateRequest()
    {
        return new CreateProductRequest
        {
            Title = "Backpack",
            Price = 109.95m,
            Description = "Travel backpack",
            Category = "bags",
            Image = "https://image.test/backpack.png",
            Rating = new CreateProductRatingRequest
            {
                Rate = 4.5m,
                Count = 12
            }
        };
    }
}
