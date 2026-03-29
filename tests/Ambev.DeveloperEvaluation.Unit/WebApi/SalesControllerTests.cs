using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

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
}
