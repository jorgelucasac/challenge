using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class ProductCreatedEvent : INotification
{
    public int ProductId { get; }
    public string Title { get; }
    public string Category { get; }

    public ProductCreatedEvent(Product product)
    {
        ProductId = product.Id;
        Title = product.Title;
        Category = product.Category;
    }
}
