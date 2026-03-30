using Ambev.DeveloperEvaluation.Application.StoreUsers.EventHandlers;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class StoreUserDomainEventHandlersTests
{
    [Fact]
    public async Task Handle_StoreUserCreatedEvent_LogsInformation()
    {
        var logger = Substitute.For<ILogger<StoreUserCreatedEventHandler>>();
        var handler = new StoreUserCreatedEventHandler(logger);
        var user = BuildUser();

        await handler.Handle(new StoreUserCreatedEvent(user), CancellationToken.None);

        logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            Arg.Any<Exception?>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public async Task Handle_StoreUserModifiedEvent_LogsInformation()
    {
        var logger = Substitute.For<ILogger<StoreUserModifiedEventHandler>>();
        var handler = new StoreUserModifiedEventHandler(logger);
        var user = BuildUser();

        await handler.Handle(new StoreUserModifiedEvent(user), CancellationToken.None);

        logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            Arg.Any<Exception?>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public async Task Handle_StoreUserDeletedEvent_LogsInformation()
    {
        var logger = Substitute.For<ILogger<StoreUserDeletedEventHandler>>();
        var handler = new StoreUserDeletedEventHandler(logger);

        await handler.Handle(new StoreUserDeletedEvent(1, "john", "john@example.com"), CancellationToken.None);

        logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            Arg.Any<Exception?>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    private static StoreUser BuildUser()
    {
        var user = StoreUser.Create(
            "john@example.com",
            "john",
            "secret",
            "John",
            "Doe",
            "Sao Paulo",
            "Main Street",
            10,
            "01000-000",
            "-23.5",
            "-46.6",
            "11999999999",
            UserStatus.Active,
            UserRole.Customer);

        typeof(StoreUser).GetProperty(nameof(StoreUser.Id))!.SetValue(user, 1);
        return user;
    }
}
