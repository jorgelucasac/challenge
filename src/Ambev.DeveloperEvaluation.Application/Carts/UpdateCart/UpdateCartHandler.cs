using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;

public class UpdateCartHandler : IRequestHandler<UpdateCartCommand, CartResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCartHandler(ICartRepository cartRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CartResult> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByIdForUpdateAsync(request.Id, cancellationToken);
        if (cart == null)
        {
            throw new KeyNotFoundException($"Cart with id {request.Id} was not found.");
        }

        cart.Update(
            request.UserId,
            request.Date,
            request.Products.Select(product => new CartItemInput(product.ProductId, product.Quantity)));

        await _cartRepository.UpdateAsync(cart, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        return _mapper.Map<CartResult>(cart);
    }
}
