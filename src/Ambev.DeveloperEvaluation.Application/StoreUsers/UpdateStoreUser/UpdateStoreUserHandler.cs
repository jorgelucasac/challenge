using Ambev.DeveloperEvaluation.Application.StoreUsers.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.StoreUsers.UpdateStoreUser;

public class UpdateStoreUserHandler : IRequestHandler<UpdateStoreUserCommand, StoreUserResult>
{
    private readonly IStoreUserRepository _storeUserRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateStoreUserHandler(IStoreUserRepository storeUserRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _storeUserRepository = storeUserRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<StoreUserResult> Handle(UpdateStoreUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _storeUserRepository.GetByIdForUpdateAsync(request.Id, cancellationToken);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {request.Id} was not found.");
        }

        user.Update(
            request.Email,
            request.Username,
            request.Password,
            request.Firstname,
            request.Lastname,
            request.City,
            request.Street,
            request.Number,
            request.Zipcode,
            request.Lat,
            request.Long,
            request.Phone,
            request.Status,
            request.Role);

        await _storeUserRepository.UpdateAsync(user, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        return _mapper.Map<StoreUserResult>(user);
    }
}
