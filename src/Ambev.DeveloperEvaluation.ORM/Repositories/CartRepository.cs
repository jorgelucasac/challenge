using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class CartRepository : ICartRepository
{
    private readonly DefaultContext _context;

    public CartRepository(DefaultContext context)
    {
        _context = context;
    }

    public async Task<Cart> CreateAsync(Cart cart, CancellationToken cancellationToken = default)
    {
        await _context.Carts.AddAsync(cart, cancellationToken);
        return cart;
    }

    public Task<Cart?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _context.Carts
            .AsNoTracking()
            .Include(cart => cart.Products)
            .FirstOrDefaultAsync(cart => cart.Id == id, cancellationToken);
    }

    public Task<Cart?> GetByIdForUpdateAsync(int id, CancellationToken cancellationToken = default)
    {
        return _context.Carts
            .Include(cart => cart.Products)
            .FirstOrDefaultAsync(cart => cart.Id == id, cancellationToken);
    }

    public async Task<PagedResult<Cart>> ListAsync(CartListFilter filter, CancellationToken cancellationToken = default)
    {
        IQueryable<Cart> query = _context.Carts
            .AsNoTracking()
            .Include(cart => cart.Products);

        query = ApplyOrdering(query, filter.Order);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((filter.Page - 1) * filter.Size)
            .Take(filter.Size)
            .ToListAsync(cancellationToken);

        return new PagedResult<Cart>(items, filter.Page, filter.Size, totalCount);
    }

    public Task<Cart> UpdateAsync(Cart cart, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(cart);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var cart = await _context.Carts
            .Include(current => current.Products)
            .FirstOrDefaultAsync(current => current.Id == id, cancellationToken);

        if (cart == null)
        {
            return false;
        }

        cart.MarkAsDeleted();
        _context.Carts.Remove(cart);
        return true;
    }

    private static IQueryable<Cart> ApplyOrdering(IQueryable<Cart> query, IReadOnlyList<CartSortField> order)
    {
        if (order.Count == 0)
        {
            return query.OrderBy(cart => cart.Id);
        }

        IOrderedQueryable<Cart>? orderedQuery = null;

        foreach (var sortField in order)
        {
            orderedQuery = orderedQuery == null
                ? ApplyInitialOrdering(sortField, query)
                : ApplySubsequentOrdering(sortField, orderedQuery);
        }

        return orderedQuery ?? query.OrderBy(cart => cart.Id);
    }

    private static IOrderedQueryable<Cart> ApplyInitialOrdering(CartSortField sortField, IQueryable<Cart> query)
    {
        return sortField.Field.ToLowerInvariant() switch
        {
            "id" => sortField.Descending ? query.OrderByDescending(cart => cart.Id) : query.OrderBy(cart => cart.Id),
            "userid" => sortField.Descending ? query.OrderByDescending(cart => cart.UserId) : query.OrderBy(cart => cart.UserId),
            "date" => sortField.Descending ? query.OrderByDescending(cart => cart.Date) : query.OrderBy(cart => cart.Date),
            _ => sortField.Descending ? query.OrderByDescending(cart => cart.Id) : query.OrderBy(cart => cart.Id)
        };
    }

    private static IOrderedQueryable<Cart> ApplySubsequentOrdering(CartSortField sortField, IOrderedQueryable<Cart> query)
    {
        return sortField.Field.ToLowerInvariant() switch
        {
            "id" => sortField.Descending ? query.ThenByDescending(cart => cart.Id) : query.ThenBy(cart => cart.Id),
            "userid" => sortField.Descending ? query.ThenByDescending(cart => cart.UserId) : query.ThenBy(cart => cart.UserId),
            "date" => sortField.Descending ? query.ThenByDescending(cart => cart.Date) : query.ThenBy(cart => cart.Date),
            _ => sortField.Descending ? query.ThenByDescending(cart => cart.Id) : query.ThenBy(cart => cart.Id)
        };
    }
}
