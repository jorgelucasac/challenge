using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface IStoreUserRepository
{
    Task<StoreUser> CreateAsync(StoreUser user, CancellationToken cancellationToken = default);
    Task<StoreUser?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<StoreUser?> GetByIdForUpdateAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<StoreUser>> ListAsync(StoreUserListFilter filter, CancellationToken cancellationToken = default);
    Task<StoreUser> UpdateAsync(StoreUser user, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
