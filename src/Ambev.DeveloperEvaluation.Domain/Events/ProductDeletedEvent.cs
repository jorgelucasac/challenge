using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class ProductDeletedEvent : INotification
{
    public int ProductId { get; }
    public string Title { get; }
    public string Category { get; }

    public ProductDeletedEvent(int productId, string title, string category)
    {
        ProductId = productId;
        Title = title;
        Category = category;
    }
}
