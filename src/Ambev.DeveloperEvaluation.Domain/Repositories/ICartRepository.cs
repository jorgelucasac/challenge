using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface ICartRepository
{
    Task<Cart> CreateAsync(Cart cart, CancellationToken cancellationToken = default);
    Task<Cart?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Cart?> GetByIdForUpdateAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<Cart>> ListAsync(CartListFilter filter, CancellationToken cancellationToken = default);
    Task<Cart> UpdateAsync(Cart cart, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
