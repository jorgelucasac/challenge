using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a single item within a sale and encapsulates discount calculations.
/// </summary>
public class SaleItem : BaseEntity
{
    public Guid SaleId { get; private set; }
    public string ProductExternalId { get; private set; } = string.Empty;
    public string ProductName { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal DiscountPercent { get; private set; }
    public decimal DiscountAmount { get; private set; }
    public decimal TotalAmount { get; private set; }
    public bool IsCancelled { get; private set; }

    private SaleItem()
    {
    }

    internal SaleItem(
        Guid saleId,
        string productExternalId,
        string productName,
        int quantity,
        decimal unitPrice)
    {
        Id = Guid.NewGuid();
        SaleId = saleId;
        Update(quantity, unitPrice, productName, productExternalId);
    }

    public void Update(int quantity, decimal unitPrice, string productName, string productExternalId)
    {
        Quantity = quantity;
        UnitPrice = unitPrice;
        ProductName = productName;
        ProductExternalId = productExternalId;

        Recalculate();
    }

    public void Cancel()
    {
        if (IsCancelled)
        {
            return;
        }

        IsCancelled = true;
        DiscountAmount = 0;
        TotalAmount = 0;
    }

    public ValidationResultDetail Validate()
    {
        var validator = new SaleItemValidator();
        var result = validator.Validate(this);

        return new ValidationResultDetail(result);
    }

    private void Recalculate()
    {
        EnsureBusinessRules();

        var grossAmount = Quantity * UnitPrice;
        DiscountPercent = ResolveDiscountPercent(Quantity);
        DiscountAmount = grossAmount * (DiscountPercent / 100m);
        TotalAmount = grossAmount - DiscountAmount;

        if (IsCancelled)
        {
            TotalAmount = 0;
            DiscountAmount = 0;
        }
    }

    private void EnsureBusinessRules()
    {
        if (Quantity <= 0)
        {
            throw new DomainException("Item quantity must be greater than zero.");
        }

        if (UnitPrice <= 0)
        {
            throw new DomainException("Item unit price must be greater than zero.");
        }

        if (Quantity > 20)
        {
            throw new DomainException("It is not possible to sell more than 20 identical items.");
        }

        if (string.IsNullOrWhiteSpace(ProductExternalId))
        {
            throw new DomainException("Product external id is required.");
        }

        if (string.IsNullOrWhiteSpace(ProductName))
        {
            throw new DomainException("Product name is required.");
        }
    }

    private static decimal ResolveDiscountPercent(int quantity)
    {
        if (quantity >= 10)
        {
            return 20m;
        }

        if (quantity >= 4)
        {
            return 10m;
        }

        return 0m;
    }
}
