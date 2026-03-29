using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly ISaleNumberGenerator _saleNumberGenerator;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateSaleHandler> _logger;
    private readonly CreateSaleHandler _handler;

    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _saleNumberGenerator = Substitute.For<ISaleNumberGenerator>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<CreateSaleHandler>>();
        _handler = new CreateSaleHandler(_saleRepository, _saleNumberGenerator, _mapper, _logger);
    }

    [Fact(DisplayName = "Given valid sale data When creating sale Then persists and returns created sale")]
    public async Task Handle_ValidRequest_ReturnsCreatedSale()
    {
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var sale = CreateSale(command, "SALE-123");
        var result = new CreateSaleResult { Id = sale.Id, SaleNumber = sale.SaleNumber, TotalAmount = sale.TotalAmount };

        _saleNumberGenerator.Generate().Returns("SALE-123");
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(result);

        var createSaleResult = await _handler.Handle(command, CancellationToken.None);

        createSaleResult.Id.Should().Be(sale.Id);
        createSaleResult.SaleNumber.Should().Be("SALE-123");
        await _saleRepository.Received(1).CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    private static Sale CreateSale(CreateSaleCommand command, string saleNumber)
    {
        return Sale.Create(
            saleNumber,
            DateTime.UtcNow,
            command.CustomerExternalId,
            command.CustomerName,
            command.BranchExternalId,
            command.BranchName,
            command.Items.Select(item => new SaleItemInput(item.ProductExternalId, item.ProductName, item.Quantity, item.UnitPrice)));
    }
}
