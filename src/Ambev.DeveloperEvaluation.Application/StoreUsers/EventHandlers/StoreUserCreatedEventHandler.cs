using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.StoreUsers.EventHandlers;

public class StoreUserCreatedEventHandler : INotificationHandler<StoreUserCreatedEvent>
{
    private readonly ILogger<StoreUserCreatedEventHandler> _logger;

    public StoreUserCreatedEventHandler(ILogger<StoreUserCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(StoreUserCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "DomainEventReceived: EventName={EventName}, UserId={UserId}, Email={Email}",
            nameof(StoreUserCreatedEvent),
            notification.UserId,
            notification.Email);

        return Task.CompletedTask;
    }
}
