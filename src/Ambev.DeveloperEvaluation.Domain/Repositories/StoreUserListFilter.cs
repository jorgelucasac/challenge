namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public class StoreUserListFilter
{
    public int Page { get; init; }
    public int Size { get; init; }
    public IReadOnlyList<StoreUserSortField> Order { get; init; } = [];
}
