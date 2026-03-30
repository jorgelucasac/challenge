using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using NSubstitute;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class DeleteProductHandlerTests
{
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    [Fact(DisplayName = "Given existing product When deleting Then commits changes")]
    public async Task Handle_ExistingProduct_DeletesProduct()
    {
        var handler = new DeleteProductHandler(_productRepository, _unitOfWork);
        _productRepository.DeleteAsync(10, Arg.Any<CancellationToken>()).Returns(true);

        await handler.Handle(new DeleteProductCommand(10), CancellationToken.None);

        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given missing product When deleting Then throws key not found")]
    public async Task Handle_MissingProduct_ThrowsKeyNotFoundException()
    {
        var handler = new DeleteProductHandler(_productRepository, _unitOfWork);
        _productRepository.DeleteAsync(10, Arg.Any<CancellationToken>()).Returns(false);

        var act = () => handler.Handle(new DeleteProductCommand(10), CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
