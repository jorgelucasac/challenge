using MediatR;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

public class ListSalesCommand : IRequest<PagedResult<ListSaleResultItem>>
{
    public int Page { get; set; } = ListSalesDefaults.DefaultPage;
    public int Size { get; set; } = ListSalesDefaults.DefaultPageSize;
    public string Order { get; set; } = ListSalesOrderParser.Default;
    public string? SaleNumber { get; set; }
    public string? CustomerName { get; set; }
    public string? BranchName { get; set; }
    public bool? IsCancelled { get; set; }
    public DateTime? SaleDateFrom { get; set; }
    public DateTime? SaleDateTo { get; set; }
}
