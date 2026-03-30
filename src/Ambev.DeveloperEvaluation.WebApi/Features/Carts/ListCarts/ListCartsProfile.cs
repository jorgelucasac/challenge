using Ambev.DeveloperEvaluation.Application.Carts.ListCarts;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.ListCarts;

public class ListCartsProfile : Profile
{
    public ListCartsProfile()
    {
        CreateMap<ListCartsRequest, ListCartsCommand>()
            .ForMember(destination => destination.Page, options => options.MapFrom(source => source._page))
            .ForMember(destination => destination.Size, options => options.MapFrom(source => source._size))
            .ForMember(destination => destination.Order, options => options.MapFrom(source => source._order));
    }
}
