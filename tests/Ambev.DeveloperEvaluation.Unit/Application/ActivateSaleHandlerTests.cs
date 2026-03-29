using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Sales.ActivateSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class ActivateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ActivateSaleHandler _handler;

    public ActivateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mapper = Substitute.For<IMapper>();
        _handler = new ActivateSaleHandler(_saleRepository, _unitOfWork, _mapper);
    }

    [Fact(DisplayName = "Given cancelled sale When activating Then returns active sale")]
    public async Task Handle_CancelledSale_ReturnsActivatedSale()
    {
        var sale = Sale.Create(
            "SALE-203",
            DateTime.UtcNow,
            "customer-1",
            "Customer",
            "branch-1",
            "Branch",
            [new SaleItemInput("product-1", "Product", 2, 10m)]);
        sale.Cancel();
        var result = new CreateSaleResult { Id = sale.Id, SaleNumber = sale.SaleNumber, IsCancelled = false, TotalAmount = 20m };

        _saleRepository.GetByIdForUpdateAsync(sale.Id, Arg.Any<CancellationToken>()).Returns(sale);
        _saleRepository.UpdateAsync(sale, Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(result);

        var response = await _handler.Handle(new ActivateSaleCommand(sale.Id), CancellationToken.None);

        response.IsCancelled.Should().BeFalse();
        sale.IsCancelled.Should().BeFalse();
        sale.TotalAmount.Should().BeGreaterThan(0m);
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given active sale When activating Then operation is idempotent")]
    public async Task Handle_AlreadyActiveSale_ReturnsActiveSale()
    {
        var sale = Sale.Create(
            "SALE-204",
            DateTime.UtcNow,
            "customer-1",
            "Customer",
            "branch-1",
            "Branch",
            [new SaleItemInput("product-1", "Product", 2, 10m)]);
        var result = new CreateSaleResult { Id = sale.Id, SaleNumber = sale.SaleNumber, IsCancelled = false, TotalAmount = 20m };

        _saleRepository.GetByIdForUpdateAsync(sale.Id, Arg.Any<CancellationToken>()).Returns(sale);
        _saleRepository.UpdateAsync(sale, Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(result);

        var response = await _handler.Handle(new ActivateSaleCommand(sale.Id), CancellationToken.None);

        response.IsCancelled.Should().BeFalse();
        sale.IsCancelled.Should().BeFalse();
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }
}
