using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleCancelledEvent : INotification
{
    public Guid SaleId { get; }
    public string SaleNumber { get; }

    public SaleCancelledEvent(Guid saleId, string saleNumber)
    {
        SaleId = saleId;
        SaleNumber = saleNumber;
    }
}
