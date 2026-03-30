using Ambev.DeveloperEvaluation.Application.StoreUsers.ListStoreUsers;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.ListStoreUsers;

public class ListStoreUsersProfile : Profile
{
    public ListStoreUsersProfile()
    {
        CreateMap<ListStoreUsersRequest, ListStoreUsersCommand>()
            .ForMember(destination => destination.Page, options => options.MapFrom(source => source._page))
            .ForMember(destination => destination.Size, options => options.MapFrom(source => source._size))
            .ForMember(destination => destination.Order, options => options.MapFrom(source => source._order));
    }
}
