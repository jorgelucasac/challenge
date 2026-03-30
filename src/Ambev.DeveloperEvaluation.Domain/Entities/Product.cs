using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Product : IHasDomainEvents
{
    private readonly List<INotification> _domainEvents = [];

    public int Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;
    public string Image { get; private set; } = string.Empty;
    public ProductRating Rating { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

    private Product()
    {
    }

    private Product(
        string title,
        decimal price,
        string description,
        string category,
        string image,
        ProductRating rating)
    {
        CreatedAt = DateTime.UtcNow;
        Title = title;
        Price = price;
        Description = description;
        Category = category;
        Image = image;
        Rating = rating;
    }

    public static Product Create(
        string title,
        decimal price,
        string description,
        string category,
        string image,
        decimal rate,
        int count)
    {
        var product = new Product(
            title,
            price,
            description,
            category,
            image,
            new ProductRating(rate, count));

        product.EnsureValid();
        product.AddDomainEvent(new ProductCreatedEvent(product));
        return product;
    }

    public void Update(
        string title,
        decimal price,
        string description,
        string category,
        string image,
        decimal rate,
        int count)
    {
        Title = title;
        Price = price;
        Description = description;
        Category = category;
        Image = image;
        Rating.Update(rate, count);
        UpdatedAt = DateTime.UtcNow;

        EnsureValid();
        AddDomainEvent(new ProductModifiedEvent(this));
    }

    public void MarkAsDeleted()
    {
        AddDomainEvent(new ProductDeletedEvent(Id, Title, Category));
    }

    public ValidationResultDetail Validate()
    {
        var validator = new ProductValidator();
        var result = validator.Validate(this);

        return new ValidationResultDetail(result);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    private void AddDomainEvent(INotification domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    private void EnsureValid()
    {
        var validation = Validate();
        if (!validation.IsValid)
        {
            throw new DomainException(validation.Errors.First().Detail);
        }
    }
}
