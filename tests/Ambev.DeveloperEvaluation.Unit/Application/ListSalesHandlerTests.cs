using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class ListSalesHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ListSalesHandler _handler;

    public ListSalesHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new ListSalesHandler(_saleRepository, _mapper);
    }

    [Fact(DisplayName = "Given list sales command When handling Then returns paged result")]
    public async Task Handle_ValidCommand_ReturnsPagedSales()
    {
        var command = new ListSalesCommand
        {
            Page = 2,
            Size = 5,
            Order = "saleNumber_asc",
            SaleNumber = "SALE",
            CustomerName = "Customer",
            BranchName = "Branch",
            IsCancelled = false
        };
        var sale = Sale.Create(
            "SALE-001",
            DateTime.UtcNow,
            "customer-1",
            "Customer 1",
            "branch-1",
            "Branch 1",
            [new SaleItemInput("product-1", "Product 1", 2, 10m)]);
        var itemResult = new ListSaleResultItem
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            CustomerName = sale.CustomerName
        };
        var pagedSales = new PagedResult<Sale>([sale], command.Page, command.Size, 11);

        _saleRepository.ListAsync(
            Arg.Is<ListSalesFilter>(filter =>
                filter.Page == command.Page &&
                filter.Size == command.Size &&
                filter.Order == SaleSortOrder.SaleNumberAscending &&
                filter.SaleNumber == command.SaleNumber &&
                filter.CustomerName == command.CustomerName &&
                filter.BranchName == command.BranchName &&
                filter.IsCancelled == command.IsCancelled),
            Arg.Any<CancellationToken>())
            .Returns(pagedSales);
        _mapper.Map<List<ListSaleResultItem>>(pagedSales.Items).Returns([itemResult]);

        var response = await _handler.Handle(command, CancellationToken.None);

        response.CurrentPage.Should().Be(command.Page);
        response.PageSize.Should().Be(command.Size);
        response.TotalCount.Should().Be(11);
        response.Items.Should().ContainSingle();
        response.Items[0].SaleNumber.Should().Be("SALE-001");
    }

    [Fact(DisplayName = "Given command with trimmed filters When handling Then trims filter values")]
    public async Task Handle_CommandWithWhitespace_TrimsFilterValues()
    {
        var command = new ListSalesCommand
        {
            SaleNumber = "  SALE-001  ",
            CustomerName = "  Customer  ",
            BranchName = "  Branch  "
        };
        var pagedSales = new PagedResult<Sale>([], command.Page, command.Size, 0);

        _saleRepository.ListAsync(
            Arg.Is<ListSalesFilter>(filter =>
                filter.Page == ListSalesDefaults.DefaultPage &&
                filter.Size == ListSalesDefaults.DefaultPageSize &&
                filter.Order == SaleSortOrder.SaleDateDescending &&
                filter.SaleNumber == "SALE-001" &&
                filter.CustomerName == "Customer" &&
                filter.BranchName == "Branch"),
            Arg.Any<CancellationToken>())
            .Returns(pagedSales);
        _mapper.Map<List<ListSaleResultItem>>(pagedSales.Items).Returns([]);

        var response = await _handler.Handle(command, CancellationToken.None);

        response.Items.Should().BeEmpty();
        response.CurrentPage.Should().Be(ListSalesDefaults.DefaultPage);
        response.PageSize.Should().Be(ListSalesDefaults.DefaultPageSize);
    }
}
