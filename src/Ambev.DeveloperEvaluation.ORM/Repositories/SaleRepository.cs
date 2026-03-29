using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;

    public SaleRepository(DefaultContext context)
    {
        _context = context;
    }

    public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        await _context.Sales.AddAsync(sale, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .AsNoTracking()
            .Include(sale => sale.Items)
            .FirstOrDefaultAsync(sale => sale.Id == id, cancellationToken);
    }

    public async Task<Sale?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .Include(sale => sale.Items)
            .FirstOrDefaultAsync(sale => sale.Id == id, cancellationToken);
    }

    public async Task<PagedResult<Sale>> ListAsync(ListSalesFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Sales.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SaleNumber))
        {
            query = query.Where(sale =>
                EF.Functions.ILike(sale.SaleNumber, $"%{filter.SaleNumber}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.CustomerName))
        {
            query = query.Where(sale =>
                EF.Functions.ILike(sale.CustomerName, $"%{filter.CustomerName}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.BranchName))
        {
            query = query.Where(sale =>
                EF.Functions.ILike(sale.BranchName, $"%{filter.BranchName}%"));
        }

        if (filter.IsCancelled.HasValue)
        {
            query = query.Where(sale => sale.IsCancelled == filter.IsCancelled.Value);
        }

        if (filter.SaleDateFrom.HasValue)
        {
            query = query.Where(sale => sale.SaleDate >= filter.SaleDateFrom.Value);
        }

        if (filter.SaleDateTo.HasValue)
        {
            query = query.Where(sale => sale.SaleDate <= filter.SaleDateTo.Value);
        }

        query = filter.Order switch
        {
            SaleSortOrder.SaleDateAscending => query.OrderBy(sale => sale.SaleDate),
            SaleSortOrder.SaleDateDescending => query.OrderByDescending(sale => sale.SaleDate),
            SaleSortOrder.SaleNumberAscending => query.OrderBy(sale => sale.SaleNumber),
            SaleSortOrder.SaleNumberDescending => query.OrderByDescending(sale => sale.SaleNumber),
            SaleSortOrder.CustomerNameAscending => query.OrderBy(sale => sale.CustomerName),
            SaleSortOrder.CustomerNameDescending => query.OrderByDescending(sale => sale.CustomerName),
            SaleSortOrder.BranchNameAscending => query.OrderBy(sale => sale.BranchName),
            SaleSortOrder.BranchNameDescending => query.OrderByDescending(sale => sale.BranchName),
            SaleSortOrder.TotalAmountAscending => query.OrderBy(sale => sale.TotalAmount),
            SaleSortOrder.TotalAmountDescending => query.OrderByDescending(sale => sale.TotalAmount),
            _ => query.OrderByDescending(sale => sale.SaleDate)
        };

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((filter.Page - 1) * filter.Size)
            .Take(filter.Size)
            .ToListAsync(cancellationToken);

        return new PagedResult<Sale>(items, filter.Page, filter.Size, totalCount);
    }

    public async Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sale = await _context.Sales.FirstOrDefaultAsync(sale => sale.Id == id, cancellationToken);
        if (sale == null)
        {
            return false;
        }

        _context.Sales.Remove(sale);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
