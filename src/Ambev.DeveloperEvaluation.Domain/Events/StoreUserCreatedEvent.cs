using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class StoreUserCreatedEvent : INotification
{
    private readonly StoreUser _storeUser;

    public int UserId => _storeUser.Id;
    public string Username => _storeUser.Username;
    public string Email => _storeUser.Email;

    public StoreUserCreatedEvent(StoreUser storeUser)
    {
        _storeUser = storeUser;
    }
}
