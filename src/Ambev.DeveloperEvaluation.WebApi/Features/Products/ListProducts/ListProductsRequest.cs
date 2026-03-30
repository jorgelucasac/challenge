namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;

public class ListProductsRequest
{
    public int? _page { get; set; }
    public int? _size { get; set; }
    public string? _order { get; set; }
    public string? Title { get; set; }
    public string? Category { get; set; }
    public decimal? Price { get; set; }
    public decimal? _minPrice { get; set; }
    public decimal? _maxPrice { get; set; }
}
