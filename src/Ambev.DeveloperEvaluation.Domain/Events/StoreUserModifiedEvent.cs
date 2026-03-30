using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class StoreUserModifiedEvent : INotification
{
    public int UserId { get; }
    public string Username { get; }
    public string Email { get; }

    public StoreUserModifiedEvent(StoreUser storeUser)
    {
        UserId = storeUser.Id;
        Username = storeUser.Username;
        Email = storeUser.Email;
    }
}
