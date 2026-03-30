using Ambev.DeveloperEvaluation.Application.StoreUsers.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.StoreUsers.GetStoreUser;

public class GetStoreUserHandler : IRequestHandler<GetStoreUserCommand, StoreUserResult>
{
    private readonly IStoreUserRepository _storeUserRepository;
    private readonly IMapper _mapper;

    public GetStoreUserHandler(IStoreUserRepository storeUserRepository, IMapper mapper)
    {
        _storeUserRepository = storeUserRepository;
        _mapper = mapper;
    }

    public async Task<StoreUserResult> Handle(GetStoreUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _storeUserRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {request.Id} was not found.");
        }

        return _mapper.Map<StoreUserResult>(user);
    }
}
