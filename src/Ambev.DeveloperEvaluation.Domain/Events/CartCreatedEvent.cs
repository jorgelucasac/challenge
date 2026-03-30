using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class CartCreatedEvent : INotification
{
    private readonly Cart _cart;

    public int CartId => _cart.Id;
    public int UserId => _cart.UserId;

    public CartCreatedEvent(Cart cart)
    {
        _cart = cart;
    }
}
