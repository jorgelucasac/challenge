using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;

public class DeleteCartHandler : IRequestHandler<DeleteCartCommand>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCartHandler(ICartRepository cartRepository, IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteCartCommand request, CancellationToken cancellationToken)
    {
        var deleted = await _cartRepository.DeleteAsync(request.Id, cancellationToken);
        if (!deleted)
        {
            throw new KeyNotFoundException($"Cart with id {request.Id} was not found.");
        }

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
