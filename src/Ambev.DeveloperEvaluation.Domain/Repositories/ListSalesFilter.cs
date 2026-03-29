namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public class ListSalesFilter
{
    public int Page { get; init; }
    public int Size { get; init; }
    public SaleSortOrder Order { get; init; }
    public string? SaleNumber { get; init; }
    public string? CustomerName { get; init; }
    public string? BranchName { get; init; }
    public bool? IsCancelled { get; init; }
    public DateTime? SaleDateFrom { get; init; }
    public DateTime? SaleDateTo { get; init; }
}
