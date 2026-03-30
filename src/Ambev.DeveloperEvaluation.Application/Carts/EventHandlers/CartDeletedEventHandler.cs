using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Carts.EventHandlers;

public class CartDeletedEventHandler : INotificationHandler<CartDeletedEvent>
{
    private readonly ILogger<CartDeletedEventHandler> _logger;

    public CartDeletedEventHandler(ILogger<CartDeletedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(CartDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "DomainEventReceived: EventName={EventName}, CartId={CartId}, UserId={UserId}",
            nameof(CartDeletedEvent),
            notification.CartId,
            notification.UserId);

        return Task.CompletedTask;
    }
}
