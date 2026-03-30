using Ambev.DeveloperEvaluation.Application.StoreUsers.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.StoreUsers.DeleteStoreUser;

public class DeleteStoreUserHandler : IRequestHandler<DeleteStoreUserCommand, StoreUserResult>
{
    private readonly IStoreUserRepository _storeUserRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteStoreUserHandler(IStoreUserRepository storeUserRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _storeUserRepository = storeUserRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<StoreUserResult> Handle(DeleteStoreUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _storeUserRepository.GetByIdForUpdateAsync(request.Id, cancellationToken);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {request.Id} was not found.");
        }

        var result = _mapper.Map<StoreUserResult>(user);
        await _storeUserRepository.DeleteAsync(request.Id, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        return result;
    }
}
