namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

public class ListSaleResultItem
{
    public Guid Id { get; set; }
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public string CustomerExternalId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string BranchExternalId { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public bool IsCancelled { get; set; }
}
