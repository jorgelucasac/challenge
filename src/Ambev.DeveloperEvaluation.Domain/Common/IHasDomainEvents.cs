using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Common;

public interface IHasDomainEvents
{
    IReadOnlyCollection<INotification> DomainEvents { get; }
    void ClearDomainEvents();
}
