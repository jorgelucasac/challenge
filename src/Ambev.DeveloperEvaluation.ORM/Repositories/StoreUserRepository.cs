using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class StoreUserRepository : IStoreUserRepository
{
    private readonly DefaultContext _context;

    public StoreUserRepository(DefaultContext context)
    {
        _context = context;
    }

    public async Task<StoreUser> CreateAsync(StoreUser user, CancellationToken cancellationToken = default)
    {
        await _context.StoreUsers.AddAsync(user, cancellationToken);
        return user;
    }

    public Task<StoreUser?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _context.StoreUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
    }

    public Task<StoreUser?> GetByIdForUpdateAsync(int id, CancellationToken cancellationToken = default)
    {
        return _context.StoreUsers
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
    }

    public async Task<PagedResult<StoreUser>> ListAsync(StoreUserListFilter filter, CancellationToken cancellationToken = default)
    {
        IQueryable<StoreUser> query = _context.StoreUsers.AsNoTracking();
        query = ApplyOrdering(query, filter.Order);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((filter.Page - 1) * filter.Size)
            .Take(filter.Size)
            .ToListAsync(cancellationToken);

        return new PagedResult<StoreUser>(items, filter.Page, filter.Size, totalCount);
    }

    public Task<StoreUser> UpdateAsync(StoreUser user, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(user);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await _context.StoreUsers.FirstOrDefaultAsync(current => current.Id == id, cancellationToken);
        if (user == null)
        {
            return false;
        }

        user.MarkAsDeleted();
        _context.StoreUsers.Remove(user);
        return true;
    }

    private static IQueryable<StoreUser> ApplyOrdering(IQueryable<StoreUser> query, IReadOnlyList<StoreUserSortField> order)
    {
        if (order.Count == 0)
        {
            return query.OrderBy(user => user.Id);
        }

        IOrderedQueryable<StoreUser>? orderedQuery = null;

        foreach (var sortField in order)
        {
            orderedQuery = orderedQuery == null
                ? ApplyInitialOrdering(sortField, query)
                : ApplySubsequentOrdering(sortField, orderedQuery);
        }

        return orderedQuery ?? query.OrderBy(user => user.Id);
    }

    private static IOrderedQueryable<StoreUser> ApplyInitialOrdering(StoreUserSortField sortField, IQueryable<StoreUser> query)
    {
        return sortField.Field.ToLowerInvariant() switch
        {
            "id" => sortField.Descending ? query.OrderByDescending(user => user.Id) : query.OrderBy(user => user.Id),
            "email" => sortField.Descending ? query.OrderByDescending(user => user.Email) : query.OrderBy(user => user.Email),
            "username" => sortField.Descending ? query.OrderByDescending(user => user.Username) : query.OrderBy(user => user.Username),
            "password" => sortField.Descending ? query.OrderByDescending(user => user.Password) : query.OrderBy(user => user.Password),
            "phone" => sortField.Descending ? query.OrderByDescending(user => user.Phone) : query.OrderBy(user => user.Phone),
            "status" => sortField.Descending ? query.OrderByDescending(user => user.Status) : query.OrderBy(user => user.Status),
            "role" => sortField.Descending ? query.OrderByDescending(user => user.Role) : query.OrderBy(user => user.Role),
            _ => sortField.Descending ? query.OrderByDescending(user => user.Id) : query.OrderBy(user => user.Id)
        };
    }

    private static IOrderedQueryable<StoreUser> ApplySubsequentOrdering(StoreUserSortField sortField, IOrderedQueryable<StoreUser> query)
    {
        return sortField.Field.ToLowerInvariant() switch
        {
            "id" => sortField.Descending ? query.ThenByDescending(user => user.Id) : query.ThenBy(user => user.Id),
            "email" => sortField.Descending ? query.ThenByDescending(user => user.Email) : query.ThenBy(user => user.Email),
            "username" => sortField.Descending ? query.ThenByDescending(user => user.Username) : query.ThenBy(user => user.Username),
            "password" => sortField.Descending ? query.ThenByDescending(user => user.Password) : query.ThenBy(user => user.Password),
            "phone" => sortField.Descending ? query.ThenByDescending(user => user.Phone) : query.ThenBy(user => user.Phone),
            "status" => sortField.Descending ? query.ThenByDescending(user => user.Status) : query.ThenBy(user => user.Status),
            "role" => sortField.Descending ? query.ThenByDescending(user => user.Role) : query.ThenBy(user => user.Role),
            _ => sortField.Descending ? query.ThenByDescending(user => user.Id) : query.ThenBy(user => user.Id)
        };
    }
}
