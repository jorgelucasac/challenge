using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Domain.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.ListCarts;

public class ListCartsCommand : IRequest<PagedResult<CartResult>>
{
    public int Page { get; set; } = ListCartsDefaults.DefaultPage;
    public int Size { get; set; } = ListCartsDefaults.DefaultPageSize;
    public string Order { get; set; } = ListCartsDefaults.DefaultOrder;
}
