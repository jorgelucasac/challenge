using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class CartModifiedEvent : INotification
{
    public int CartId { get; }
    public int UserId { get; }

    public CartModifiedEvent(Cart cart)
    {
        CartId = cart.Id;
        UserId = cart.UserId;
    }
}
