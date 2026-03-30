using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Carts.EventHandlers;

public class CartCreatedEventHandler : INotificationHandler<CartCreatedEvent>
{
    private readonly ILogger<CartCreatedEventHandler> _logger;

    public CartCreatedEventHandler(ILogger<CartCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(CartCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "DomainEventReceived: EventName={EventName}, CartId={CartId}, UserId={UserId}",
            nameof(CartCreatedEvent),
            notification.CartId,
            notification.UserId);

        return Task.CompletedTask;
    }
}
