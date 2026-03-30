using Ambev.DeveloperEvaluation.Application.StoreUsers.Common;
using Ambev.DeveloperEvaluation.Domain.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.StoreUsers.ListStoreUsers;

public class ListStoreUsersCommand : IRequest<PagedResult<StoreUserResult>>
{
    public int Page { get; set; } = ListStoreUsersDefaults.DefaultPage;
    public int Size { get; set; } = ListStoreUsersDefaults.DefaultPageSize;
    public string Order { get; set; } = ListStoreUsersDefaults.DefaultOrder;
}
