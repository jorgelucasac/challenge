using Ambev.DeveloperEvaluation.Application.StoreUsers.Common;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.StoreUsers.ListStoreUsers;

public class ListStoreUsersHandler : IRequestHandler<ListStoreUsersCommand, PagedResult<StoreUserResult>>
{
    private readonly IStoreUserRepository _storeUserRepository;
    private readonly IMapper _mapper;

    public ListStoreUsersHandler(IStoreUserRepository storeUserRepository, IMapper mapper)
    {
        _storeUserRepository = storeUserRepository;
        _mapper = mapper;
    }

    public async Task<PagedResult<StoreUserResult>> Handle(ListStoreUsersCommand request, CancellationToken cancellationToken)
    {
        var filter = new StoreUserListFilter
        {
            Page = request.Page,
            Size = request.Size,
            Order = ListStoreUsersOrderParser.Parse(request.Order)
        };

        var pagedUsers = await _storeUserRepository.ListAsync(filter, cancellationToken);
        var items = _mapper.Map<List<StoreUserResult>>(pagedUsers.Items);

        return new PagedResult<StoreUserResult>(items, pagedUsers.CurrentPage, pagedUsers.PageSize, pagedUsers.TotalCount);
    }
}
