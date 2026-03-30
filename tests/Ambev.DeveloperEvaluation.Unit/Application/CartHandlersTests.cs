using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;
using Ambev.DeveloperEvaluation.Application.Carts.GetCart;
using Ambev.DeveloperEvaluation.Application.Carts.ListCarts;
using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CartHandlersTests
{
    private readonly ICartRepository _repository = Substitute.For<ICartRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();

    [Fact]
    public async Task CreateCart_ShouldPersistAndCommit()
    {
        var handler = new CreateCartHandler(_repository, _unitOfWork, _mapper);
        var command = new CreateCartCommand
        {
            UserId = 1,
            Date = new DateTime(2026, 3, 30, 0, 0, 0, DateTimeKind.Utc),
            Products = [new CreateCartProductInput { ProductId = 10, Quantity = 2 }]
        };
        var result = new CartResult { Id = 1, UserId = 1 };

        _mapper.Map<CartResult>(Arg.Any<Cart>()).Returns(result);

        var response = await handler.Handle(command, CancellationToken.None);

        response.UserId.Should().Be(1);
        await _repository.Received(1).CreateAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetCart_ShouldReturnMappedCart()
    {
        var handler = new GetCartHandler(_repository, _mapper);
        var cart = BuildCart();
        var result = new CartResult { Id = 1, UserId = 1 };

        _repository.GetByIdAsync(1, Arg.Any<CancellationToken>()).Returns(cart);
        _mapper.Map<CartResult>(cart).Returns(result);

        var response = await handler.Handle(new GetCartCommand(1), CancellationToken.None);

        response.Id.Should().Be(1);
    }

    [Fact]
    public async Task ListCarts_ShouldReturnPagedResult()
    {
        var handler = new ListCartsHandler(_repository, _mapper);
        var carts = new List<Cart> { BuildCart() };
        var mapped = new List<CartResult> { new() { Id = 1, UserId = 1 } };

        _repository.ListAsync(Arg.Any<CartListFilter>(), Arg.Any<CancellationToken>())
            .Returns(new PagedResult<Cart>(carts, 1, 10, 1));
        _mapper.Map<List<CartResult>>(carts).Returns(mapped);

        var response = await handler.Handle(new ListCartsCommand(), CancellationToken.None);

        response.TotalCount.Should().Be(1);
        response.Items.Should().ContainSingle();
    }

    [Fact]
    public async Task UpdateCart_ShouldUpdateAndCommit()
    {
        var handler = new UpdateCartHandler(_repository, _unitOfWork, _mapper);
        var cart = BuildCart();
        var command = new UpdateCartCommand
        {
            Id = 1,
            UserId = 2,
            Date = new DateTime(2026, 3, 31, 0, 0, 0, DateTimeKind.Utc),
            Products = [new UpdateCartProductInput { ProductId = 11, Quantity = 3 }]
        };
        var result = new CartResult { Id = 1, UserId = 2 };

        _repository.GetByIdForUpdateAsync(1, Arg.Any<CancellationToken>()).Returns(cart);
        _mapper.Map<CartResult>(cart).Returns(result);

        var response = await handler.Handle(command, CancellationToken.None);

        response.UserId.Should().Be(2);
        await _repository.Received(1).UpdateAsync(cart, Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteCart_ShouldDeleteAndCommit()
    {
        var handler = new DeleteCartHandler(_repository, _unitOfWork);
        _repository.DeleteAsync(1, Arg.Any<CancellationToken>()).Returns(true);

        await handler.Handle(new DeleteCartCommand(1), CancellationToken.None);

        await _repository.Received(1).DeleteAsync(1, Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    private static Cart BuildCart()
    {
        var cart = Cart.Create(
            1,
            new DateTime(2026, 3, 30, 0, 0, 0, DateTimeKind.Utc),
            [new CartItemInput(10, 2)]);

        typeof(Cart).GetProperty(nameof(Cart.Id))!.SetValue(cart, 1);
        return cart;
    }
}
