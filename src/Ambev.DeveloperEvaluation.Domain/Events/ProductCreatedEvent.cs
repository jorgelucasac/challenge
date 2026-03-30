using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class ProductCreatedEvent : INotification
{
    private readonly Product _product;

    public int ProductId => _product.Id;
    public string Title => _product.Title;
    public string Category => _product.Category;

    public ProductCreatedEvent(Product product)
    {
        _product = product;
    }
}
