using Ambev.DeveloperEvaluation.Application.Sales.EventHandlers;
using Ambev.DeveloperEvaluation.Domain.Events;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class SaleDomainEventHandlersTests
{
    [Fact(DisplayName = "Sale created event handler should log structured event")]
    public async Task Handle_SaleCreatedEvent_LogsInformation()
    {
        var logger = Substitute.For<ILogger<SaleCreatedEventHandler>>();
        var handler = new SaleCreatedEventHandler(logger);

        await handler.Handle(new SaleCreatedEvent(Guid.NewGuid(), "SALE-300"), CancellationToken.None);

        logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            Arg.Any<Exception?>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact(DisplayName = "Sale modified event handler should log structured event")]
    public async Task Handle_SaleModifiedEvent_LogsInformation()
    {
        var logger = Substitute.For<ILogger<SaleModifiedEventHandler>>();
        var handler = new SaleModifiedEventHandler(logger);

        await handler.Handle(new SaleModifiedEvent(Guid.NewGuid(), "SALE-301"), CancellationToken.None);

        logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            Arg.Any<Exception?>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact(DisplayName = "Sale cancelled event handler should log structured event")]
    public async Task Handle_SaleCancelledEvent_LogsInformation()
    {
        var logger = Substitute.For<ILogger<SaleCancelledEventHandler>>();
        var handler = new SaleCancelledEventHandler(logger);

        await handler.Handle(new SaleCancelledEvent(Guid.NewGuid(), "SALE-302"), CancellationToken.None);

        logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            Arg.Any<Exception?>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact(DisplayName = "Item cancelled event handler should log structured event")]
    public async Task Handle_ItemCancelledEvent_LogsInformation()
    {
        var logger = Substitute.For<ILogger<ItemCancelledEventHandler>>();
        var handler = new ItemCancelledEventHandler(logger);

        await handler.Handle(new ItemCancelledEvent(Guid.NewGuid(), Guid.NewGuid(), "SALE-303"), CancellationToken.None);

        logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            Arg.Any<Exception?>(),
            Arg.Any<Func<object, Exception?, string>>());
    }
}
