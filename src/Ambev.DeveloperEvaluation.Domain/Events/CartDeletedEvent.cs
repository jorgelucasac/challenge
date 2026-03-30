using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class CartDeletedEvent : INotification
{
    public int CartId { get; }
    public int UserId { get; }

    public CartDeletedEvent(int cartId, int userId)
    {
        CartId = cartId;
        UserId = userId;
    }
}
