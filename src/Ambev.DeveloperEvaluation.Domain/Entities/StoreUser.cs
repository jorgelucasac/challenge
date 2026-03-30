using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class StoreUser : IHasDomainEvents
{
    private readonly List<INotification> _domainEvents = [];

    public int Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string Username { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;
    public StoreUserName Name { get; private set; } = null!;
    public StoreUserAddress Address { get; private set; } = null!;
    public string Phone { get; private set; } = string.Empty;
    public UserStatus Status { get; private set; }
    public UserRole Role { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

    private StoreUser()
    {
    }

    private StoreUser(
        string email,
        string username,
        string password,
        StoreUserName name,
        StoreUserAddress address,
        string phone,
        UserStatus status,
        UserRole role)
    {
        Email = email;
        Username = username;
        Password = password;
        Name = name;
        Address = address;
        Phone = phone;
        Status = status;
        Role = role;
        CreatedAt = DateTime.UtcNow;
    }

    public static StoreUser Create(
        string email,
        string username,
        string password,
        string firstname,
        string lastname,
        string city,
        string street,
        int number,
        string zipcode,
        string lat,
        string @long,
        string phone,
        UserStatus status,
        UserRole role)
    {
        var user = new StoreUser(
            email,
            username,
            password,
            new StoreUserName(firstname, lastname),
            new StoreUserAddress(city, street, number, zipcode, new StoreUserGeolocation(lat, @long)),
            phone,
            status,
            role);

        user.EnsureValid();
        user.AddDomainEvent(new StoreUserCreatedEvent(user));
        return user;
    }

    public void Update(
        string email,
        string username,
        string password,
        string firstname,
        string lastname,
        string city,
        string street,
        int number,
        string zipcode,
        string lat,
        string @long,
        string phone,
        UserStatus status,
        UserRole role)
    {
        Email = email;
        Username = username;
        Password = password;
        Name.Update(firstname, lastname);
        Address.Update(city, street, number, zipcode, new StoreUserGeolocation(lat, @long));
        Phone = phone;
        Status = status;
        Role = role;
        UpdatedAt = DateTime.UtcNow;

        EnsureValid();
        AddDomainEvent(new StoreUserModifiedEvent(this));
    }

    public void MarkAsDeleted()
    {
        AddDomainEvent(new StoreUserDeletedEvent(Id, Username, Email));
    }

    public ValidationResultDetail Validate()
    {
        var validator = new StoreUserValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail(result);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
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
