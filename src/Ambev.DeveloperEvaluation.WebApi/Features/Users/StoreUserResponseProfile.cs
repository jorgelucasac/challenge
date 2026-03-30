using Ambev.DeveloperEvaluation.Application.StoreUsers.Common;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users;

public class StoreUserResponseProfile : Profile
{
    public StoreUserResponseProfile()
    {
        CreateMap<StoreUserResult, StoreUserResponse>()
            .ForMember(destination => destination.Status, options => options.MapFrom(source => source.Status.ToString()))
            .ForMember(destination => destination.Role, options => options.MapFrom(source => source.Role.ToString()));
        CreateMap<StoreUserNameResult, StoreUserNameResponse>();
        CreateMap<StoreUserAddressResult, StoreUserAddressResponse>();
        CreateMap<StoreUserGeolocationResult, StoreUserGeolocationResponse>();
    }
}
