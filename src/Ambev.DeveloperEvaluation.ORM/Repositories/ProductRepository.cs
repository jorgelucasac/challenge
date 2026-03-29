using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly DefaultContext _context;

    public ProductRepository(DefaultContext context)
    {
        _context = context;
    }

    public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _context.Products.AddAsync(product, cancellationToken);
        return product;
    }

    public Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(product => product.Id == id, cancellationToken);
    }

    public Task<Product?> GetByIdForUpdateAsync(int id, CancellationToken cancellationToken = default)
    {
        return _context.Products
            .FirstOrDefaultAsync(product => product.Id == id, cancellationToken);
    }

    public async Task<PagedResult<Product>> ListAsync(ProductListFilter filter, CancellationToken cancellationToken = default)
    {
        IQueryable<Product> query = _context.Products.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Title))
        {
            query = ApplyStringFilter(query, product => product.Title, filter.Title);
        }

        if (!string.IsNullOrWhiteSpace(filter.Category))
        {
            query = ApplyStringFilter(query, product => product.Category, filter.Category);
        }

        if (filter.ExactPrice.HasValue)
        {
            query = query.Where(product => product.Price == filter.ExactPrice.Value);
        }

        if (filter.MinPrice.HasValue)
        {
            query = query.Where(product => product.Price >= filter.MinPrice.Value);
        }

        if (filter.MaxPrice.HasValue)
        {
            query = query.Where(product => product.Price <= filter.MaxPrice.Value);
        }

        query = ApplyOrdering(query, filter.Order);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((filter.Page - 1) * filter.Size)
            .Take(filter.Size)
            .ToListAsync(cancellationToken);

        return new PagedResult<Product>(items, filter.Page, filter.Size, totalCount);
    }

    public async Task<IReadOnlyList<string>> ListCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .AsNoTracking()
            .Select(product => product.Category)
            .Distinct()
            .OrderBy(category => category)
            .ToListAsync(cancellationToken);
    }

    public Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(product);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await _context.Products.FirstOrDefaultAsync(current => current.Id == id, cancellationToken);
        if (product == null)
        {
            return false;
        }

        product.MarkAsDeleted();
        _context.Products.Remove(product);
        return true;
    }

    private static IQueryable<Product> ApplyStringFilter(
        IQueryable<Product> query,
        System.Linq.Expressions.Expression<Func<Product, string>> selector,
        string value)
    {
        if (value.StartsWith('*') && value.EndsWith('*') && value.Length > 1)
        {
            var normalized = value.Trim('*');
            return query.Where(product => EF.Functions.ILike(EF.Property<string>(product, GetPropertyName(selector)), $"%{normalized}%"));
        }

        if (value.StartsWith('*'))
        {
            var normalized = value.TrimStart('*');
            return query.Where(product => EF.Functions.ILike(EF.Property<string>(product, GetPropertyName(selector)), $"%{normalized}"));
        }

        if (value.EndsWith('*'))
        {
            var normalized = value.TrimEnd('*');
            return query.Where(product => EF.Functions.ILike(EF.Property<string>(product, GetPropertyName(selector)), $"{normalized}%"));
        }

        return query.Where(product => EF.Functions.ILike(EF.Property<string>(product, GetPropertyName(selector)), value));
    }

    private static IQueryable<Product> ApplyOrdering(IQueryable<Product> query, IReadOnlyList<ProductSortField> order)
    {
        if (order.Count == 0)
        {
            return query.OrderBy(product => product.Id);
        }

        IOrderedQueryable<Product>? orderedQuery = null;

        foreach (var sortField in order)
        {
            orderedQuery = ApplyOrdering(sortField, orderedQuery ?? query);
        }

        return orderedQuery ?? query.OrderBy(product => product.Id);
    }

    private static IOrderedQueryable<Product> ApplyOrdering(ProductSortField sortField, IQueryable<Product> query)
    {
        return sortField.Field.ToLowerInvariant() switch
        {
            "id" => sortField.Descending ? query.OrderByDescending(product => product.Id) : query.OrderBy(product => product.Id),
            "title" => sortField.Descending ? query.OrderByDescending(product => product.Title) : query.OrderBy(product => product.Title),
            "price" => sortField.Descending ? query.OrderByDescending(product => product.Price) : query.OrderBy(product => product.Price),
            "description" => sortField.Descending ? query.OrderByDescending(product => product.Description) : query.OrderBy(product => product.Description),
            "category" => sortField.Descending ? query.OrderByDescending(product => product.Category) : query.OrderBy(product => product.Category),
            "image" => sortField.Descending ? query.OrderByDescending(product => product.Image) : query.OrderBy(product => product.Image),
            "rate" => sortField.Descending ? query.OrderByDescending(product => product.Rating.Rate) : query.OrderBy(product => product.Rating.Rate),
            "count" => sortField.Descending ? query.OrderByDescending(product => product.Rating.Count) : query.OrderBy(product => product.Rating.Count),
            _ => sortField.Descending ? query.OrderByDescending(product => product.Id) : query.OrderBy(product => product.Id)
        };
    }

    private static string GetPropertyName(System.Linq.Expressions.Expression<Func<Product, string>> selector)
    {
        return ((System.Linq.Expressions.MemberExpression)selector.Body).Member.Name;
    }
}
