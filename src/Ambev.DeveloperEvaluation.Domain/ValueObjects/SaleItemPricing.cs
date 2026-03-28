namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public sealed record SaleItemPricing(
    decimal GrossAmount,
    decimal DiscountPercent,
    decimal DiscountAmount,
    decimal TotalAmount);
