namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;

public class ListSalesRequest
{
    public int? _page { get; set; }
    public int? _size { get; set; }
    public string? _order { get; set; }
    public string? SaleNumber { get; set; }
    public string? CustomerName { get; set; }
    public string? BranchName { get; set; }
    public bool? IsCancelled { get; set; }
    public DateTime? SaleDateFrom { get; set; }
    public DateTime? SaleDateTo { get; set; }
}
