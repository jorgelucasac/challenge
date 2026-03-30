using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.ListCarts;

public class ListCartsHandler : IRequestHandler<ListCartsCommand, PagedResult<CartResult>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    public ListCartsHandler(ICartRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    public async Task<PagedResult<CartResult>> Handle(ListCartsCommand request, CancellationToken cancellationToken)
    {
        var filter = new CartListFilter
        {
            Page = request.Page,
            Size = request.Size,
            Order = ListCartsOrderParser.Parse(request.Order)
        };

        var pagedCarts = await _cartRepository.ListAsync(filter, cancellationToken);
        var items = _mapper.Map<List<CartResult>>(pagedCarts.Items);

        return new PagedResult<CartResult>(items, pagedCarts.CurrentPage, pagedCarts.PageSize, pagedCarts.TotalCount);
    }
}
