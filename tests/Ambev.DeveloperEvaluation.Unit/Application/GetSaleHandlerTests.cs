using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class GetSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly GetSaleHandler _handler;

    public GetSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetSaleHandler(_saleRepository, _mapper);
    }

    [Fact(DisplayName = "Given existing sale When getting by id Then returns sale")]
    public async Task Handle_ExistingSale_ReturnsSale()
    {
        var saleId = Guid.NewGuid();
        var sale = Sale.Create(
            "SALE-100",
            DateTime.UtcNow,
            "customer-1",
            "Customer",
            "branch-1",
            "Branch",
            [new SaleItemInput("product-1", "Product", 2, 10m)]);
        var result = new CreateSaleResult { Id = saleId, SaleNumber = sale.SaleNumber };

        sale.Id = saleId;
        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(result);

        var response = await _handler.Handle(new GetSaleCommand(saleId), CancellationToken.None);

        response.Id.Should().Be(saleId);
        response.SaleNumber.Should().Be(sale.SaleNumber);
    }

    [Fact(DisplayName = "Given missing sale When getting by id Then throws not found")]
    public async Task Handle_MissingSale_ThrowsKeyNotFoundException()
    {
        var saleId = Guid.NewGuid();
        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns((Sale?)null);

        var act = () => _handler.Handle(new GetSaleCommand(saleId), CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
