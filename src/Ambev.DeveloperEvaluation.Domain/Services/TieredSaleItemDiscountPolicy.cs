using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Services;

public class TieredSaleItemDiscountPolicy : ISaleItemDiscountPolicy
{
    private const int MinimumQuantity = 1;
    private const int MaximumQuantity = 20;
    private const decimal NoDiscountPercent = 0m;
    private const decimal TenPercentDiscount = 10m;
    private const decimal TwentyPercentDiscount = 20m;

    private static readonly DiscountTier[] DiscountTiers =
    [
        new(MinimumQuantity, 3, NoDiscountPercent),
        new(4, 9, TenPercentDiscount),
        new(10, MaximumQuantity, TwentyPercentDiscount)
    ];

    public SaleItemPricing Calculate(int quantity, decimal unitPrice)
    {
        if (quantity < MinimumQuantity)
        {
            throw new DomainException("Item quantity must be greater than zero.");
        }

        if (quantity > MaximumQuantity)
        {
            throw new DomainException("It is not possible to sell more than 20 identical items.");
        }

        if (unitPrice <= 0)
        {
            throw new DomainException("Item unit price must be greater than zero.");
        }

        var grossAmount = quantity * unitPrice;
        var discountPercent = ResolveDiscountPercent(quantity);
        var discountAmount = grossAmount * (discountPercent / 100m);
        var totalAmount = grossAmount - discountAmount;

        return new SaleItemPricing(grossAmount, discountPercent, discountAmount, totalAmount);
    }

    private static decimal ResolveDiscountPercent(int quantity)
    {
        return DiscountTiers
            .First(tier => tier.Supports(quantity))
            .DiscountPercent;
    }

    private sealed record DiscountTier(int MinimumQuantity, int MaximumQuantity, decimal DiscountPercent)
    {
        public bool Supports(int quantity)
        {
            return quantity >= MinimumQuantity && quantity <= MaximumQuantity;
        }
    }
}
