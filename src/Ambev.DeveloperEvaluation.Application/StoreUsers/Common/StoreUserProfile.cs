using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.StoreUsers.Common;

public class StoreUserProfile : Profile
{
    public StoreUserProfile()
    {
        CreateMap<StoreUser, StoreUserResult>();
        CreateMap<StoreUserName, StoreUserNameResult>();
        CreateMap<StoreUserAddress, StoreUserAddressResult>();
        CreateMap<StoreUserGeolocation, StoreUserGeolocationResult>();
    }
}
