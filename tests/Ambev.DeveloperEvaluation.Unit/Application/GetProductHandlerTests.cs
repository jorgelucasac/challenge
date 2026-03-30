using Ambev.DeveloperEvaluation.Application.Products.Common;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class GetProductHandlerTests
{
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();

    [Fact(DisplayName = "Given existing product When getting by id Then returns product")]
    public async Task Handle_ExistingProduct_ReturnsProduct()
    {
        var handler = new GetProductHandler(_productRepository, _mapper);
        var product = Product.Create("Backpack", 109.95m, "Travel backpack", "bags", "https://image.test/backpack.png", 4.5m, 12);
        var result = new ProductResult { Id = 1, Title = "Backpack" };

        _productRepository.GetByIdAsync(1, Arg.Any<CancellationToken>()).Returns(product);
        _mapper.Map<ProductResult>(product).Returns(result);

        var response = await handler.Handle(new GetProductCommand(1), CancellationToken.None);

        response.Id.Should().Be(1);
        response.Title.Should().Be("Backpack");
    }

    [Fact(DisplayName = "Given missing product When getting by id Then throws key not found")]
    public async Task Handle_MissingProduct_ThrowsKeyNotFoundException()
    {
        var handler = new GetProductHandler(_productRepository, _mapper);
        _productRepository.GetByIdAsync(1, Arg.Any<CancellationToken>()).Returns((Product?)null);

        var act = () => handler.Handle(new GetProductCommand(1), CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
