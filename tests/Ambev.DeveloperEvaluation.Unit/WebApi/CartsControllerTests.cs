using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.Carts.GetCart;
using Ambev.DeveloperEvaluation.Application.Carts.ListCarts;
using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.ListCarts;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace Ambev.DeveloperEvaluation.Unit.WebApi;

public class CartsControllerTests
{
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly CartsController _controller;

    public CartsControllerTests()
    {
        _controller = new CartsController(_mediator, _mapper);
    }

    [Fact]
    public async Task CreateCart_ShouldReturnCreatedPayload()
    {
        var request = BuildCreateRequest();
        var command = new CreateCartCommand();
        var result = new CartResult { Id = 1, UserId = 1 };
        var response = new CartResponse { Id = 1, UserId = 1 };

        _mapper.Map<CreateCartCommand>(request).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>()).Returns(result);
        _mapper.Map<CartResponse>(result).Returns(response);

        var actionResult = await _controller.CreateCart(request, CancellationToken.None);

        var created = actionResult.Should().BeOfType<CreatedAtActionResult>().Subject;
        created.Value.Should().Be(response);
    }

    [Fact]
    public async Task GetCart_ShouldReturnOkPayload()
    {
        var result = new CartResult { Id = 1, UserId = 1 };
        var response = new CartResponse { Id = 1, UserId = 1 };

        _mediator.Send(Arg.Any<GetCartCommand>(), Arg.Any<CancellationToken>()).Returns(result);
        _mapper.Map<CartResponse>(result).Returns(response);

        var actionResult = await _controller.GetCart(1, CancellationToken.None);

        var ok = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().Be(response);
    }

    [Fact]
    public async Task ListCarts_ShouldReturnPagedPayload()
    {
        var request = new ListCartsRequest { _page = 1, _size = 10, _order = "id desc, userId asc" };
        var command = new ListCartsCommand();
        var result = new PagedResult<CartResult>([new CartResult { Id = 1, UserId = 1 }], 1, 10, 1);
        var response = new List<CartResponse> { new() { Id = 1, UserId = 1 } };

        _mapper.Map<ListCartsCommand>(request).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>()).Returns(result);
        _mapper.Map<List<CartResponse>>(result.Items).Returns(response);

        var actionResult = await _controller.ListCarts(request, CancellationToken.None);

        var ok = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeOfType<PagedCartsResponse>().Which.TotalItems.Should().Be(1);
    }

    [Fact]
    public async Task UpdateCart_ShouldReturnOkPayload()
    {
        var request = new UpdateCartRequest
        {
            UserId = 2,
            Date = new DateTime(2026, 3, 31, 0, 0, 0, DateTimeKind.Utc),
            Products = [new UpdateCartProductRequest { ProductId = 11, Quantity = 3 }]
        };
        var command = new UpdateCartCommand();
        var result = new CartResult { Id = 1, UserId = 2 };
        var response = new CartResponse { Id = 1, UserId = 2 };

        _mapper.Map<UpdateCartCommand>(Arg.Any<UpdateCartRequest>()).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>()).Returns(result);
        _mapper.Map<CartResponse>(result).Returns(response);

        var actionResult = await _controller.UpdateCart(1, request, CancellationToken.None);

        var ok = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().Be(response);
    }

    [Fact]
    public async Task DeleteCart_ShouldReturnMessagePayload()
    {
        var actionResult = await _controller.DeleteCart(1, CancellationToken.None);

        var ok = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeOfType<DeleteCartResponse>().Which.Message.Should().Be("Cart deleted successfully");
    }

    private static CreateCartRequest BuildCreateRequest()
    {
        return new CreateCartRequest
        {
            UserId = 1,
            Date = new DateTime(2026, 3, 30, 0, 0, 0, DateTimeKind.Utc),
            Products = [new CreateCartProductRequest { ProductId = 10, Quantity = 2 }]
        };
    }
}
