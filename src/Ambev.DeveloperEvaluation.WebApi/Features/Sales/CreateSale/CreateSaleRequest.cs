namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public class CreateSaleRequest
{
    public DateTime SaleDate { get; set; }
    public string CustomerExternalId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string BranchExternalId { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;
    public List<CreateSaleItemRequest> Items { get; set; } = [];
}
