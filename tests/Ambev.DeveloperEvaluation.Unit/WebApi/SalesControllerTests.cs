using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Sales.ActivateSale;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ActivateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

namespace Ambev.DeveloperEvaluation.Unit.WebApi;

public class SalesControllerTests
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly SalesController _controller;

    public SalesControllerTests()
    {
        _mediator = Substitute.For<IMediator>();
        _mapper = Substitute.For<IMapper>();
        _controller = new SalesController(_mediator, _mapper);
    }

    [Fact(DisplayName = "Given valid request When creating sale Then returns created response")]
    public async Task CreateSale_ValidRequest_ReturnsCreatedResult()
    {
        var request = new CreateSaleRequest
        {
            SaleDate = DateTime.UtcNow,
            CustomerExternalId = "customer-1",
            CustomerName = "Customer",
            BranchExternalId = "branch-1",
            BranchName = "Branch",
            Items = [new CreateSaleItemRequest { ProductExternalId = "product-1", ProductName = "Product", Quantity = 1, UnitPrice = 10m }]
        };
        var command = new CreateSaleCommand();
        var result = new CreateSaleResult { Id = Guid.NewGuid(), SaleNumber = "SALE-001" };
        var response = new CreateSaleResponse { Id = result.Id, SaleNumber = result.SaleNumber };

        _mapper.Map<CreateSaleCommand>(request).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>()).Returns(result);
        _mapper.Map<CreateSaleResponse>(result).Returns(response);

        var actionResult = await _controller.CreateSale(request, CancellationToken.None);

        var createdResult = actionResult.Should().BeOfType<CreatedResult>().Subject;
        var payload = createdResult.Value.Should().BeOfType<ApiResponseWithData<CreateSaleResponse>>().Subject;

        payload.Success.Should().BeTrue();
        payload.Data.Should().NotBeNull();
        payload.Data!.SaleNumber.Should().Be("SALE-001");
    }

    [Fact(DisplayName = "Given invalid request When creating sale Then returns bad request")]
    public async Task CreateSale_InvalidRequest_ReturnsBadRequest()
    {
        var actionResult = await _controller.CreateSale(new CreateSaleRequest(), CancellationToken.None);

        actionResult.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact(DisplayName = "Given valid id When getting sale Then returns ok response")]
    public async Task GetSale_ValidRequest_ReturnsOkResult()
    {
        var saleId = Guid.NewGuid();
        var command = new GetSaleCommand(saleId);
        var result = new CreateSaleResult { Id = saleId, SaleNumber = "SALE-001" };
        var response = new CreateSaleResponse { Id = saleId, SaleNumber = "SALE-001" };

        _mapper.Map<GetSaleCommand>(saleId).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>()).Returns(result);
        _mapper.Map<CreateSaleResponse>(result).Returns(response);

        var actionResult = await _controller.GetSale(saleId, CancellationToken.None);

        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        var payload = okResult.Value.Should().BeOfType<ApiResponseWithData<CreateSaleResponse>>().Subject;

        payload.Success.Should().BeTrue();
        payload.Data.Should().NotBeNull();
        payload.Data!.SaleNumber.Should().Be("SALE-001");
    }

    [Fact(DisplayName = "Given empty id When getting sale Then returns bad request")]
    public async Task GetSale_InvalidRequest_ReturnsBadRequest()
    {
        var actionResult = await _controller.GetSale(Guid.Empty, CancellationToken.None);

        actionResult.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact(DisplayName = "Given valid list request When listing sales Then returns paginated response")]
    public async Task ListSales_ValidRequest_ReturnsOkResult()
    {
        var request = new ListSalesRequest { _page = 2, _size = 5, _order = "saleDate_desc" };
        var command = new ListSalesCommand { Page = 2, Size = 5, Order = "saleDate_desc" };
        var resultItems = new List<ListSaleResultItem> { new() { Id = Guid.NewGuid(), SaleNumber = "SALE-001" } };
        var responseItems = new List<ListSaleResponse> { new() { Id = resultItems[0].Id, SaleNumber = "SALE-001" } };
        var result = new PagedResult<ListSaleResultItem>(resultItems, 2, 5, 8);

        _mapper.Map<ListSalesCommand>(request).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>()).Returns(result);
        _mapper.Map<List<ListSaleResponse>>(result.Items).Returns(responseItems);

        var actionResult = await _controller.ListSales(request, CancellationToken.None);

        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        var payload = okResult.Value.Should().BeOfType<PaginatedResponse<ListSaleResponse>>().Subject;

        payload.Success.Should().BeTrue();
        payload.CurrentPage.Should().Be(2);
        payload.TotalCount.Should().Be(8);
        payload.Data.Should().ContainSingle();
    }

    [Fact(DisplayName = "Given invalid list request When listing sales Then returns bad request")]
    public async Task ListSales_InvalidRequest_ReturnsBadRequest()
    {
        var actionResult = await _controller.ListSales(new ListSalesRequest { _page = 0 }, CancellationToken.None);

        actionResult.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact(DisplayName = "Given valid request When updating sale Then returns ok response")]
    public async Task UpdateSale_ValidRequest_ReturnsOkResult()
    {
        var saleId = Guid.NewGuid();
        var request = new UpdateSaleRequest
        {
            SaleDate = DateTime.UtcNow,
            CustomerExternalId = "customer-1",
            CustomerName = "Customer",
            BranchExternalId = "branch-1",
            BranchName = "Branch",
            Items =
            [
                new UpdateSaleItemRequest
                {
                    Id = Guid.NewGuid(),
                    ProductExternalId = "product-1",
                    ProductName = "Product",
                    Quantity = 1,
                    UnitPrice = 10m
                }
            ]
        };
        var command = new UpdateSaleCommand { Id = saleId };
        var result = new CreateSaleResult { Id = saleId, SaleNumber = "SALE-001" };
        var response = new CreateSaleResponse { Id = saleId, SaleNumber = "SALE-001" };

        _mapper.Map<UpdateSaleCommand>(Arg.Is<UpdateSaleRequest>(item => item.Id == saleId)).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>()).Returns(result);
        _mapper.Map<CreateSaleResponse>(result).Returns(response);

        var actionResult = await _controller.UpdateSale(saleId, request, CancellationToken.None);

        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        var payload = okResult.Value.Should().BeOfType<ApiResponseWithData<CreateSaleResponse>>().Subject;

        payload.Success.Should().BeTrue();
        payload.Data.Should().NotBeNull();
        payload.Data!.SaleNumber.Should().Be("SALE-001");
    }

    [Fact(DisplayName = "Given empty route id When updating sale Then returns bad request")]
    public async Task UpdateSale_InvalidRequest_ReturnsBadRequest()
    {
        var request = new UpdateSaleRequest
        {
            SaleDate = DateTime.UtcNow,
            CustomerExternalId = "customer-1",
            CustomerName = "Customer",
            BranchExternalId = "branch-1",
            BranchName = "Branch",
            Items =
            [
                new UpdateSaleItemRequest
                {
                    Id = Guid.NewGuid(),
                    ProductExternalId = "product-1",
                    ProductName = "Product",
                    Quantity = 1,
                    UnitPrice = 10m
                }
            ]
        };

        var actionResult = await _controller.UpdateSale(Guid.Empty, request, CancellationToken.None);

        actionResult.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact(DisplayName = "Given valid id When cancelling sale Then returns ok response")]
    public async Task CancelSale_ValidRequest_ReturnsOkResult()
    {
        var saleId = Guid.NewGuid();
        var command = new CancelSaleCommand(saleId);
        var result = new CreateSaleResult { Id = saleId, SaleNumber = "SALE-001", IsCancelled = true };
        var response = new CreateSaleResponse { Id = saleId, SaleNumber = "SALE-001", IsCancelled = true };

        _mapper.Map<CancelSaleCommand>(saleId).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>()).Returns(result);
        _mapper.Map<CreateSaleResponse>(result).Returns(response);

        var actionResult = await _controller.CancelSale(saleId, CancellationToken.None);

        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        var payload = okResult.Value.Should().BeOfType<ApiResponseWithData<CreateSaleResponse>>().Subject;

        payload.Success.Should().BeTrue();
        payload.Data!.IsCancelled.Should().BeTrue();
    }

    [Fact(DisplayName = "Given valid id When activating sale Then returns ok response")]
    public async Task ActivateSale_ValidRequest_ReturnsOkResult()
    {
        var saleId = Guid.NewGuid();
        var command = new ActivateSaleCommand(saleId);
        var result = new CreateSaleResult { Id = saleId, SaleNumber = "SALE-001", IsCancelled = false };
        var response = new CreateSaleResponse { Id = saleId, SaleNumber = "SALE-001", IsCancelled = false };

        _mapper.Map<ActivateSaleCommand>(saleId).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>()).Returns(result);
        _mapper.Map<CreateSaleResponse>(result).Returns(response);

        var actionResult = await _controller.ActivateSale(saleId, CancellationToken.None);

        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        var payload = okResult.Value.Should().BeOfType<ApiResponseWithData<CreateSaleResponse>>().Subject;

        payload.Success.Should().BeTrue();
        payload.Data!.IsCancelled.Should().BeFalse();
    }

    [Fact(DisplayName = "Given valid id When deleting sale Then returns ok response")]
    public async Task DeleteSale_ValidRequest_ReturnsOkResult()
    {
        var saleId = Guid.NewGuid();
        var command = new DeleteSaleCommand(saleId);
        var result = new DeleteSaleResponse { Success = true };

        _mapper.Map<DeleteSaleCommand>(saleId).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>()).Returns(result);

        var actionResult = await _controller.DeleteSale(saleId, CancellationToken.None);

        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        var payload = okResult.Value.Should().BeOfType<ApiResponse>().Subject;

        payload.Success.Should().BeTrue();
        payload.Message.Should().Be("Sale deleted successfully");
    }

    [Fact(DisplayName = "Given empty id When deleting sale Then returns bad request")]
    public async Task DeleteSale_InvalidRequest_ReturnsBadRequest()
    {
        var actionResult = await _controller.DeleteSale(Guid.Empty, CancellationToken.None);

        actionResult.Should().BeOfType<BadRequestObjectResult>();
    }
}
