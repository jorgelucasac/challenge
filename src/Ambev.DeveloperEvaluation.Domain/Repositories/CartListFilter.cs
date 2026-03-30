namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public class CartListFilter
{
    public int Page { get; init; }
    public int Size { get; init; }
    public IReadOnlyList<CartSortField> Order { get; init; } = [];
}
