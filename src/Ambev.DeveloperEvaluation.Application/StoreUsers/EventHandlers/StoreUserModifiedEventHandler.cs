using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.StoreUsers.EventHandlers;

public class StoreUserModifiedEventHandler : INotificationHandler<StoreUserModifiedEvent>
{
    private readonly ILogger<StoreUserModifiedEventHandler> _logger;

    public StoreUserModifiedEventHandler(ILogger<StoreUserModifiedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(StoreUserModifiedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "DomainEventReceived: EventName={EventName}, UserId={UserId}, Email={Email}",
            nameof(StoreUserModifiedEvent),
            notification.UserId,
            notification.Email);

        return Task.CompletedTask;
    }
}
