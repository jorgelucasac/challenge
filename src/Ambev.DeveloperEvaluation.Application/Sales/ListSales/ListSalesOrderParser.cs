using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

public static class ListSalesOrderParser
{
    public const string Default = "saleDate_desc";

    private static readonly Dictionary<string, SaleSortOrder> Orders = new(StringComparer.OrdinalIgnoreCase)
    {
        ["saleDate_desc"] = SaleSortOrder.SaleDateDescending,
        ["saleDate_asc"] = SaleSortOrder.SaleDateAscending,
        ["saleNumber_desc"] = SaleSortOrder.SaleNumberDescending,
        ["saleNumber_asc"] = SaleSortOrder.SaleNumberAscending,
        ["customerName_desc"] = SaleSortOrder.CustomerNameDescending,
        ["customerName_asc"] = SaleSortOrder.CustomerNameAscending,
        ["branchName_desc"] = SaleSortOrder.BranchNameDescending,
        ["branchName_asc"] = SaleSortOrder.BranchNameAscending,
        ["totalAmount_desc"] = SaleSortOrder.TotalAmountDescending,
        ["totalAmount_asc"] = SaleSortOrder.TotalAmountAscending
    };

    public static IReadOnlyCollection<string> SupportedOrders => Orders.Keys.ToArray();

    public static string Normalize(string? order)
    {
        return string.IsNullOrWhiteSpace(order) ? Default : order.Trim();
    }

    public static bool IsSupported(string? order)
    {
        return Orders.ContainsKey(Normalize(order));
    }

    public static SaleSortOrder Parse(string? order)
    {
        var normalizedOrder = Normalize(order);

        if (!Orders.TryGetValue(normalizedOrder, out var sortOrder))
        {
            throw new ArgumentException($"Unsupported sort order '{normalizedOrder}'.", nameof(order));
        }

        return sortOrder;
    }
}
