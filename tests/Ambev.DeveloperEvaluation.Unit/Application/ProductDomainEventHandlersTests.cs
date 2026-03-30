using Ambev.DeveloperEvaluation.Application.Products.EventHandlers;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class ProductDomainEventHandlersTests
{
    [Fact(DisplayName = "Product created event handler should log structured event")]
    public async Task Handle_ProductCreatedEvent_LogsInformation()
    {
        var logger = Substitute.For<ILogger<ProductCreatedEventHandler>>();
        var handler = new ProductCreatedEventHandler(logger);
        var product = Product.Create("Backpack", 109.95m, "Travel backpack", "bags", "https://image.test/backpack.png", 4.5m, 12);

        await handler.Handle(new ProductCreatedEvent(product), CancellationToken.None);

        logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            Arg.Any<Exception?>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact(DisplayName = "Product modified event handler should log structured event")]
    public async Task Handle_ProductModifiedEvent_LogsInformation()
    {
        var logger = Substitute.For<ILogger<ProductModifiedEventHandler>>();
        var handler = new ProductModifiedEventHandler(logger);
        var product = Product.Create("Backpack", 109.95m, "Travel backpack", "bags", "https://image.test/backpack.png", 4.5m, 12);

        await handler.Handle(new ProductModifiedEvent(product), CancellationToken.None);

        logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            Arg.Any<Exception?>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact(DisplayName = "Product deleted event handler should log structured event")]
    public async Task Handle_ProductDeletedEvent_LogsInformation()
    {
        var logger = Substitute.For<ILogger<ProductDeletedEventHandler>>();
        var handler = new ProductDeletedEventHandler(logger);

        await handler.Handle(new ProductDeletedEvent(1, "Backpack", "bags"), CancellationToken.None);

        logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            Arg.Any<Exception?>(),
            Arg.Any<Func<object, Exception?, string>>());
    }
}
