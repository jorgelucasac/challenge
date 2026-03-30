namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;

public class ListProductsByCategoryRequest
{
    public string Category { get; set; } = string.Empty;
    public int? _page { get; set; }
    public int? _size { get; set; }
    public string? _order { get; set; }
}
