using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Services;

public interface ISaleItemDiscountPolicy
{
    SaleItemPricing Calculate(int quantity, decimal unitPrice);
}
