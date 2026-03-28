using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a single item within a sale and encapsulates discount calculations.
/// </summary>
public class SaleItem : BaseEntity
{
    private static readonly ISaleItemDiscountPolicy DiscountPolicy = new TieredSaleItemDiscountPolicy();

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
        EnsureRequiredFields();

        var pricing = DiscountPolicy.Calculate(Quantity, UnitPrice);
        DiscountPercent = pricing.DiscountPercent;
        DiscountAmount = pricing.DiscountAmount;
        TotalAmount = pricing.TotalAmount;

        if (IsCancelled)
        {
            TotalAmount = 0;
            DiscountAmount = 0;
        }
    }

    private void EnsureRequiredFields()
    {
        if (string.IsNullOrWhiteSpace(ProductExternalId))
        {
            throw new DomainException("Product external id is required.");
        }

        if (string.IsNullOrWhiteSpace(ProductName))
        {
            throw new DomainException("Product name is required.");
        }
    }
}
