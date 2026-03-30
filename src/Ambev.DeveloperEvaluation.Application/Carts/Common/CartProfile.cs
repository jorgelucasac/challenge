using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.Common;

public class CartProfile : Profile
{
    public CartProfile()
    {
        CreateMap<Cart, CartResult>();
        CreateMap<CartItem, CartProductResult>();
    }
}
