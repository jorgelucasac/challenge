using Ambev.DeveloperEvaluation.Application.Products.Common;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class UpdateProductHandlerTests
{
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();

    [Fact(DisplayName = "Given existing product When updating Then returns updated product")]
    public async Task Handle_ExistingProduct_UpdatesProduct()
    {
        var handler = new UpdateProductHandler(_productRepository, _unitOfWork, _mapper);
        var product = Product.Create("Backpack", 109.95m, "Travel backpack", "bags", "https://image.test/backpack.png", 4.5m, 12);
        var result = new ProductResult { Id = 1, Title = "Jacket" };

        _productRepository.GetByIdForUpdateAsync(1, Arg.Any<CancellationToken>()).Returns(product);
        _mapper.Map<ProductResult>(product).Returns(result);

        var response = await handler.Handle(new UpdateProductCommand
        {
            Id = 1,
            Title = "Jacket",
            Price = 89.90m,
            Description = "Winter jacket",
            Category = "clothing",
            Image = "https://image.test/jacket.png",
            RatingRate = 4.9m,
            RatingCount = 30
        }, CancellationToken.None);

        response.Title.Should().Be("Jacket");
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given missing product When updating Then throws key not found")]
    public async Task Handle_MissingProduct_ThrowsKeyNotFoundException()
    {
        var handler = new UpdateProductHandler(_productRepository, _unitOfWork, _mapper);
        _productRepository.GetByIdForUpdateAsync(1, Arg.Any<CancellationToken>()).Returns((Product?)null);

        var act = () => handler.Handle(new UpdateProductCommand
        {
            Id = 1,
            Title = "Jacket",
            Price = 89.90m,
            Description = "Winter jacket",
            Category = "clothing",
            Image = "https://image.test/jacket.png",
            RatingRate = 4.9m,
            RatingCount = 30
        }, CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
