using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.EventHandlers;

public class ProductModifiedEventHandler : INotificationHandler<ProductModifiedEvent>
{
    private readonly ILogger<ProductModifiedEventHandler> _logger;

    public ProductModifiedEventHandler(ILogger<ProductModifiedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ProductModifiedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "DomainEventReceived: EventName={EventName}, ProductId={ProductId}, Category={Category}",
            nameof(ProductModifiedEvent),
            notification.ProductId,
            notification.Category);

        return Task.CompletedTask;
    }
}
