using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Carts.EventHandlers;

public class CartModifiedEventHandler : INotificationHandler<CartModifiedEvent>
{
    private readonly ILogger<CartModifiedEventHandler> _logger;

    public CartModifiedEventHandler(ILogger<CartModifiedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(CartModifiedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "DomainEventReceived: EventName={EventName}, CartId={CartId}, UserId={UserId}",
            nameof(CartModifiedEvent),
            notification.CartId,
            notification.UserId);

        return Task.CompletedTask;
    }
}
