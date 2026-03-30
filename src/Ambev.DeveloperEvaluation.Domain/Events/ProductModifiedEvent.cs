using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class ProductModifiedEvent : INotification
{
    public int ProductId { get; }
    public string Title { get; }
    public string Category { get; }

    public ProductModifiedEvent(Product product)
    {
        ProductId = product.Id;
        Title = product.Title;
        Category = product.Category;
    }
}
