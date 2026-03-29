using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CancelSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly CancelSaleHandler _handler;

    public CancelSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CancelSaleHandler(_saleRepository, _unitOfWork, _mapper);
    }

    [Fact(DisplayName = "Given active sale When cancelling Then returns cancelled sale")]
    public async Task Handle_ActiveSale_ReturnsCancelledSale()
    {
        var sale = Sale.Create(
            "SALE-201",
            DateTime.UtcNow,
            "customer-1",
            "Customer",
            "branch-1",
            "Branch",
            [new SaleItemInput("product-1", "Product", 2, 10m)]);
        var result = new CreateSaleResult { Id = sale.Id, SaleNumber = sale.SaleNumber, IsCancelled = true, TotalAmount = 0m };

        _saleRepository.GetByIdForUpdateAsync(sale.Id, Arg.Any<CancellationToken>()).Returns(sale);
        _saleRepository.UpdateAsync(sale, Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(result);

        var response = await _handler.Handle(new CancelSaleCommand(sale.Id), CancellationToken.None);

        response.IsCancelled.Should().BeTrue();
        sale.IsCancelled.Should().BeTrue();
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given already cancelled sale When cancelling Then operation is idempotent")]
    public async Task Handle_AlreadyCancelledSale_ReturnsCancelledSale()
    {
        var sale = Sale.Create(
            "SALE-202",
            DateTime.UtcNow,
            "customer-1",
            "Customer",
            "branch-1",
            "Branch",
            [new SaleItemInput("product-1", "Product", 2, 10m)]);
        sale.Cancel();
        var result = new CreateSaleResult { Id = sale.Id, SaleNumber = sale.SaleNumber, IsCancelled = true, TotalAmount = 0m };

        _saleRepository.GetByIdForUpdateAsync(sale.Id, Arg.Any<CancellationToken>()).Returns(sale);
        _saleRepository.UpdateAsync(sale, Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(result);

        var response = await _handler.Handle(new CancelSaleCommand(sale.Id), CancellationToken.None);

        response.IsCancelled.Should().BeTrue();
        sale.IsCancelled.Should().BeTrue();
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }
}
