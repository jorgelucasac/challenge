namespace Ambev.DeveloperEvaluation.WebApi.Features.Products;

public class PagedProductsResponse
{
    public List<ProductResponse> Data { get; set; } = [];
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}
