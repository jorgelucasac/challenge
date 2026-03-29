namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public class ProductListFilter
{
    public int Page { get; init; }
    public int Size { get; init; }
    public IReadOnlyList<ProductSortField> Order { get; init; } = [];
    public string? Title { get; init; }
    public string? Category { get; init; }
    public decimal? ExactPrice { get; init; }
    public decimal? MinPrice { get; init; }
    public decimal? MaxPrice { get; init; }
}
