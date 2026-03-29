using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class UpdateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateSaleHandler> _logger;
    private readonly UpdateSaleHandler _handler;

    public UpdateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<UpdateSaleHandler>>();
        _handler = new UpdateSaleHandler(_saleRepository, _mapper, _logger);
    }

    [Fact(DisplayName = "Given existing sale When updating sale Then updates items and returns sale")]
    public async Task Handle_ValidUpdate_ReturnsUpdatedSale()
    {
        var sale = Sale.Create(
            "SALE-100",
            DateTime.UtcNow,
            "customer-1",
            "Customer One",
            "branch-1",
            "Branch One",
            [new SaleItemInput("product-1", "Product One", 2, 10m)]);
        var existingItem = sale.Items.Single();
        var command = new UpdateSaleCommand
        {
            Id = sale.Id,
            SaleDate = DateTime.UtcNow.AddDays(1),
            CustomerExternalId = "customer-2",
            CustomerName = "Customer Two",
            BranchExternalId = "branch-2",
            BranchName = "Branch Two",
            Items =
            [
                new UpdateSaleItemCommand
                {
                    Id = existingItem.Id,
                    ProductExternalId = "product-1",
                    ProductName = "Product One Updated",
                    Quantity = 4,
                    UnitPrice = 10m
                },
                new UpdateSaleItemCommand
                {
                    ProductExternalId = "product-2",
                    ProductName = "Product Two",
                    Quantity = 1,
                    UnitPrice = 15m
                }
            ]
        };
        var result = new CreateSaleResult { Id = sale.Id, SaleNumber = sale.SaleNumber, TotalAmount = 51m };

        _saleRepository.GetByIdForUpdateAsync(sale.Id, Arg.Any<CancellationToken>()).Returns(sale);
        _saleRepository.UpdateAsync(sale, Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(result);

        var response = await _handler.Handle(command, CancellationToken.None);

        response.Id.Should().Be(sale.Id);
        sale.CustomerName.Should().Be("Customer Two");
        sale.BranchName.Should().Be("Branch Two");
        sale.Items.Should().HaveCount(2);
        sale.Items.Should().Contain(item => item.ProductName == "Product One Updated" && item.Quantity == 4);
        sale.Items.Should().Contain(item => item.ProductName == "Product Two");
        await _saleRepository.Received(1).UpdateAsync(sale, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given existing sale When cancelling item Then recalculates total")]
    public async Task Handle_CancelItem_ReturnsUpdatedSale()
    {
        var sale = Sale.Create(
            "SALE-101",
            DateTime.UtcNow,
            "customer-1",
            "Customer One",
            "branch-1",
            "Branch One",
            [
                new SaleItemInput("product-1", "Product One", 2, 10m),
                new SaleItemInput("product-2", "Product Two", 4, 10m)
            ]);
        var itemToCancel = sale.Items.Last();
        var command = new UpdateSaleCommand
        {
            Id = sale.Id,
            SaleDate = sale.SaleDate,
            CustomerExternalId = sale.CustomerExternalId,
            CustomerName = sale.CustomerName,
            BranchExternalId = sale.BranchExternalId,
            BranchName = sale.BranchName,
            Items =
            [
                new UpdateSaleItemCommand
                {
                    Id = sale.Items.First().Id,
                    ProductExternalId = sale.Items.First().ProductExternalId,
                    ProductName = sale.Items.First().ProductName,
                    Quantity = sale.Items.First().Quantity,
                    UnitPrice = sale.Items.First().UnitPrice
                },
                new UpdateSaleItemCommand
                {
                    Id = itemToCancel.Id,
                    IsCancelled = true
                }
            ]
        };
        var result = new CreateSaleResult { Id = sale.Id, SaleNumber = sale.SaleNumber, TotalAmount = 20m };

        _saleRepository.GetByIdForUpdateAsync(sale.Id, Arg.Any<CancellationToken>()).Returns(sale);
        _saleRepository.UpdateAsync(sale, Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(result);

        var response = await _handler.Handle(command, CancellationToken.None);

        response.TotalAmount.Should().Be(20m);
        itemToCancel.IsCancelled.Should().BeTrue();
        sale.TotalAmount.Should().Be(20m);
    }

    [Fact(DisplayName = "Given missing sale When updating Then throws not found")]
    public async Task Handle_MissingSale_ThrowsKeyNotFoundException()
    {
        var command = new UpdateSaleCommand
        {
            Id = Guid.NewGuid(),
            SaleDate = DateTime.UtcNow,
            CustomerExternalId = "customer-1",
            CustomerName = "Customer",
            BranchExternalId = "branch-1",
            BranchName = "Branch",
            Items = [new UpdateSaleItemCommand { Id = Guid.NewGuid(), ProductExternalId = "product-1", ProductName = "Product", Quantity = 1, UnitPrice = 10m }]
        };
        _saleRepository.GetByIdForUpdateAsync(command.Id, Arg.Any<CancellationToken>()).Returns((Sale?)null);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
