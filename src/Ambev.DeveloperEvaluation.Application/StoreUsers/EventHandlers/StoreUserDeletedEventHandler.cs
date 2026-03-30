using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.StoreUsers.EventHandlers;

public class StoreUserDeletedEventHandler : INotificationHandler<StoreUserDeletedEvent>
{
    private readonly ILogger<StoreUserDeletedEventHandler> _logger;

    public StoreUserDeletedEventHandler(ILogger<StoreUserDeletedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(StoreUserDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "DomainEventReceived: EventName={EventName}, UserId={UserId}, Email={Email}",
            nameof(StoreUserDeletedEvent),
            notification.UserId,
            notification.Email);

        return Task.CompletedTask;
    }
}
