using Ambev.DeveloperEvaluation.Application.Carts.Common;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts;

public class CartResponseProfile : Profile
{
    public CartResponseProfile()
    {
        CreateMap<CartResult, CartResponse>();
        CreateMap<CartProductResult, CartProductResponse>();
    }
}
