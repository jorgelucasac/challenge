using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Cart : IHasDomainEvents
{
    private readonly List<CartItem> _products = [];
    private readonly List<INotification> _domainEvents = [];

    public int Id { get; private set; }
    public int UserId { get; private set; }
    public DateTime Date { get; private set; }
    public IReadOnlyCollection<CartItem> Products => _products.AsReadOnly();
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

    private Cart()
    {
    }

    private Cart(int userId, DateTime date)
    {
        UserId = userId;
        Date = date;
        CreatedAt = DateTime.UtcNow;
    }

    public static Cart Create(int userId, DateTime date, IEnumerable<CartItemInput> products)
    {
        var cart = new Cart(userId, date);
        cart.ReplaceProducts(products);
        cart.EnsureValid();
        cart.AddDomainEvent(new CartCreatedEvent(cart));
        return cart;
    }

    public void Update(int userId, DateTime date, IEnumerable<CartItemInput> products)
    {
        UserId = userId;
        Date = date;
        ReplaceProducts(products);
        UpdatedAt = DateTime.UtcNow;
        EnsureValid();
        AddDomainEvent(new CartModifiedEvent(this));
    }

    public void MarkAsDeleted()
    {
        AddDomainEvent(new CartDeletedEvent(Id, UserId));
    }

    public ValidationResultDetail Validate()
    {
        var validator = new CartValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail(result);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    private void ReplaceProducts(IEnumerable<CartItemInput> products)
    {
        _products.Clear();
        foreach (var product in products)
        {
            _products.Add(new CartItem(product.ProductId, product.Quantity));
        }
    }

    private void EnsureValid()
    {
        var validation = Validate();
        if (!validation.IsValid)
        {
            throw new DomainException(validation.Errors.First().Detail);
        }
    }

    private void AddDomainEvent(INotification domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
