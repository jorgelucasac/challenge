using Ambev.DeveloperEvaluation.Application.Carts.EventHandlers;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CartDomainEventHandlersTests
{
    [Fact]
    public async Task Handle_CartCreatedEvent_LogsInformation()
    {
        var logger = Substitute.For<ILogger<CartCreatedEventHandler>>();
        var handler = new CartCreatedEventHandler(logger);
        var cart = BuildCart();

        await handler.Handle(new CartCreatedEvent(cart), CancellationToken.None);

        logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            Arg.Any<Exception?>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public async Task Handle_CartModifiedEvent_LogsInformation()
    {
        var logger = Substitute.For<ILogger<CartModifiedEventHandler>>();
        var handler = new CartModifiedEventHandler(logger);
        var cart = BuildCart();

        await handler.Handle(new CartModifiedEvent(cart), CancellationToken.None);

        logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            Arg.Any<Exception?>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public async Task Handle_CartDeletedEvent_LogsInformation()
    {
        var logger = Substitute.For<ILogger<CartDeletedEventHandler>>();
        var handler = new CartDeletedEventHandler(logger);

        await handler.Handle(new CartDeletedEvent(1, 2), CancellationToken.None);

        logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            Arg.Any<Exception?>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    private static Cart BuildCart()
    {
        var cart = Cart.Create(1, DateTime.UtcNow, [new CartItemInput(10, 2)]);
        typeof(Cart).GetProperty(nameof(Cart.Id))!.SetValue(cart, 1);
        return cart;
    }
}
