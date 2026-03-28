using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

public static class SaleTestData
{
    public static Sale GenerateValidSale()
    {
        return Sale.Create(
            saleNumber: "SALE-001",
            saleDate: DateTime.UtcNow,
            customerExternalId: "customer-1",
            customerName: "Jane Doe",
            branchExternalId: "branch-1",
            branchName: "Main Branch",
            items:
            [
                new SaleItemInput("product-1", "Product One", 2, 10m)
            ]);
    }
}
