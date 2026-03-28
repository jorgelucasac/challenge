namespace Ambev.DeveloperEvaluation.Domain.Events;

public class ItemCancelledEvent
{
    public Guid SaleId { get; }
    public Guid ItemId { get; }
    public string SaleNumber { get; }

    public ItemCancelledEvent(Guid saleId, Guid itemId, string saleNumber)
    {
        SaleId = saleId;
        ItemId = itemId;
        SaleNumber = saleNumber;
    }
}
