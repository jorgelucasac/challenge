using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart;

public class GetCartHandler : IRequestHandler<GetCartCommand, CartResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    public GetCartHandler(ICartRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    public async Task<CartResult> Handle(GetCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByIdAsync(request.Id, cancellationToken);
        if (cart == null)
        {
            throw new KeyNotFoundException($"Cart with id {request.Id} was not found.");
        }

        return _mapper.Map<CartResult>(cart);
    }
}
