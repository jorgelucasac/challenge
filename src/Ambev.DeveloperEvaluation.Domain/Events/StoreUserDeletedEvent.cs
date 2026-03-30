using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class StoreUserDeletedEvent : INotification
{
    public int UserId { get; }
    public string Username { get; }
    public string Email { get; }

    public StoreUserDeletedEvent(int userId, string username, string email)
    {
        UserId = userId;
        Username = username;
        Email = email;
    }
}
