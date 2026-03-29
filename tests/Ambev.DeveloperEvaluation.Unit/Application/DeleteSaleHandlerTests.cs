using FluentAssertions;
using NSubstitute;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class DeleteSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly DeleteSaleHandler _handler;

    public DeleteSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _handler = new DeleteSaleHandler(_saleRepository);
    }

    [Fact(DisplayName = "Given existing sale When deleting Then returns success")]
    public async Task Handle_ExistingSale_ReturnsSuccess()
    {
        var saleId = Guid.NewGuid();
        _saleRepository.DeleteAsync(saleId, Arg.Any<CancellationToken>()).Returns(true);

        var response = await _handler.Handle(new DeleteSaleCommand(saleId), CancellationToken.None);

        response.Success.Should().BeTrue();
    }

    [Fact(DisplayName = "Given missing sale When deleting Then throws not found")]
    public async Task Handle_MissingSale_ThrowsKeyNotFoundException()
    {
        var saleId = Guid.NewGuid();
        _saleRepository.DeleteAsync(saleId, Arg.Any<CancellationToken>()).Returns(false);

        var act = () => _handler.Handle(new DeleteSaleCommand(saleId), CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
