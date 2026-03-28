using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Aggregate root for sales and item lifecycle operations.
/// </summary>
public class Sale : BaseEntity
{
    private readonly List<SaleItem> _items = [];

    public string SaleNumber { get; private set; } = string.Empty;
    public DateTime SaleDate { get; private set; }
    public string CustomerExternalId { get; private set; } = string.Empty;
    public string CustomerName { get; private set; } = string.Empty;
    public string BranchExternalId { get; private set; } = string.Empty;
    public string BranchName { get; private set; } = string.Empty;
    public decimal TotalAmount { get; private set; }
    public bool IsCancelled { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

    private Sale()
    {
    }

    private Sale(
        string saleNumber,
        DateTime saleDate,
        string customerExternalId,
        string customerName,
        string branchExternalId,
        string branchName)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        SaleNumber = saleNumber;
        SaleDate = saleDate;
        CustomerExternalId = customerExternalId;
        CustomerName = customerName;
        BranchExternalId = branchExternalId;
        BranchName = branchName;
    }

    public static Sale Create(
        string saleNumber,
        DateTime saleDate,
        string customerExternalId,
        string customerName,
        string branchExternalId,
        string branchName,
        IEnumerable<SaleItemInput> items)
    {
        var sale = new Sale(
            saleNumber,
            saleDate,
            customerExternalId,
            customerName,
            branchExternalId,
            branchName);

        foreach (var item in items)
        {
            sale.AddItem(item.ProductExternalId, item.ProductName, item.Quantity, item.UnitPrice);
        }

        if (sale._items.Count == 0)
        {
            throw new DomainException("A sale must have at least one item.");
        }

        sale.RecalculateTotals();
        return sale;
    }

    public SaleCreatedEvent CreateCreatedEvent()
    {
        return new SaleCreatedEvent(Id, SaleNumber);
    }

    public SaleModifiedEvent AddItem(
        string productExternalId,
        string productName,
        int quantity,
        decimal unitPrice)
    {
        EnsureSaleIsActive();

        var item = new SaleItem(Id, productExternalId, productName, quantity, unitPrice);
        _items.Add(item);
        Touch();
        RecalculateTotals();

        return new SaleModifiedEvent(Id, SaleNumber);
    }

    public SaleModifiedEvent UpdateItem(
        Guid itemId,
        int quantity,
        decimal unitPrice,
        string productName,
        string productExternalId)
    {
        EnsureSaleIsActive();

        var item = GetItem(itemId);
        if (item.IsCancelled)
        {
            throw new DomainException("Cancelled items cannot be updated.");
        }

        item.Update(quantity, unitPrice, productName, productExternalId);
        Touch();
        RecalculateTotals();

        return new SaleModifiedEvent(Id, SaleNumber);
    }

    public ItemCancelledEvent CancelItem(Guid itemId)
    {
        EnsureSaleIsActive();

        var item = GetItem(itemId);
        item.Cancel();
        Touch();
        RecalculateTotals();

        return new ItemCancelledEvent(Id, itemId, SaleNumber);
    }

    public SaleCancelledEvent Cancel()
    {
        if (IsCancelled)
        {
            return new SaleCancelledEvent(Id, SaleNumber);
        }

        foreach (var item in _items.Where(i => !i.IsCancelled))
        {
            item.Cancel();
        }

        IsCancelled = true;
        Touch();
        RecalculateTotals();

        return new SaleCancelledEvent(Id, SaleNumber);
    }

    public void RecalculateTotals()
    {
        if (IsCancelled)
        {
            TotalAmount = 0;
            return;
        }

        TotalAmount = _items
           .Where(item => !item.IsCancelled)
           .Sum(item => item.TotalAmount);
    }

    public ValidationResultDetail Validate()
    {
        var validator = new SaleValidator();
        var result = validator.Validate(this);

        return new ValidationResultDetail(result);
    }

    private SaleItem GetItem(Guid itemId)
    {
        return _items.FirstOrDefault(item => item.Id == itemId)
            ?? throw new DomainException($"Sale item with id {itemId} was not found.");
    }

    private void EnsureSaleIsActive()
    {
        if (IsCancelled)
        {
            throw new DomainException("Cancelled sales cannot be changed.");
        }
    }

    private void Touch()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}