using Ambev.DeveloperEvaluation.Application.StoreUsers.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.StoreUsers.CreateStoreUser;

public class CreateStoreUserHandler : IRequestHandler<CreateStoreUserCommand, StoreUserResult>
{
    private readonly IStoreUserRepository _storeUserRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateStoreUserHandler(IStoreUserRepository storeUserRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _storeUserRepository = storeUserRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<StoreUserResult> Handle(CreateStoreUserCommand request, CancellationToken cancellationToken)
    {
        var user = StoreUser.Create(
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

        await _storeUserRepository.CreateAsync(user, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        return _mapper.Map<StoreUserResult>(user);
    }
}
