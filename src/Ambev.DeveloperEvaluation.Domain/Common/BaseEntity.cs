using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Common;

public class BaseEntity : IComparable<BaseEntity>
{
    private readonly List<INotification> _domainEvents = [];

    public Guid Id { get; set; }
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

    public Task<IEnumerable<ValidationErrorDetail>> ValidateAsync()
    {
        return Validator.ValidateAsync(this);
    }

    public int CompareTo(BaseEntity? other)
    {
        if (other == null)
        {
            return 1;
        }

        return other.Id.CompareTo(Id);
    }

    protected void AddDomainEvent(INotification domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    protected bool HasDomainEvent<TEvent>()
        where TEvent : INotification
    {
        return _domainEvents.Any(domainEvent => domainEvent is TEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
