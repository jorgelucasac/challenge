using Ambev.DeveloperEvaluation.Application.Products.Common;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CreateProductHandlerTests
{
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();

    [Fact(DisplayName = "Given valid request When creating product Then persists and commits")]
    public async Task Handle_ValidRequest_CreatesProduct()
    {
        var handler = new CreateProductHandler(_productRepository, _unitOfWork, _mapper);
        var command = new CreateProductCommand
        {
            Title = "Backpack",
            Price = 109.95m,
            Description = "Travel backpack",
            Category = "bags",
            Image = "https://image.test/backpack.png",
            RatingRate = 4.5m,
            RatingCount = 12
        };
        var result = new ProductResult { Id = 1, Title = command.Title };

        _mapper.Map<ProductResult>(Arg.Any<Product>()).Returns(result);

        var response = await handler.Handle(command, CancellationToken.None);

        response.Title.Should().Be("Backpack");
        await _productRepository.Received(1).CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }
}
