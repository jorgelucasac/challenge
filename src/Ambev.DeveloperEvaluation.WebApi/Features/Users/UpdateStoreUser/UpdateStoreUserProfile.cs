using Ambev.DeveloperEvaluation.Application.StoreUsers.UpdateStoreUser;
using Ambev.DeveloperEvaluation.Domain.Enums;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateStoreUser;

public class UpdateStoreUserProfile : Profile
{
    public UpdateStoreUserProfile()
    {
        CreateMap<UpdateStoreUserRequest, UpdateStoreUserCommand>()
            .ForMember(destination => destination.Firstname, options => options.MapFrom(source => source.Name.Firstname))
            .ForMember(destination => destination.Lastname, options => options.MapFrom(source => source.Name.Lastname))
            .ForMember(destination => destination.City, options => options.MapFrom(source => source.Address.City))
            .ForMember(destination => destination.Street, options => options.MapFrom(source => source.Address.Street))
            .ForMember(destination => destination.Number, options => options.MapFrom(source => source.Address.Number))
            .ForMember(destination => destination.Zipcode, options => options.MapFrom(source => source.Address.Zipcode))
            .ForMember(destination => destination.Lat, options => options.MapFrom(source => source.Address.Geolocation.Lat))
            .ForMember(destination => destination.Long, options => options.MapFrom(source => source.Address.Geolocation.Long))
            .ForMember(destination => destination.Status, options => options.MapFrom(source => Enum.Parse<UserStatus>(source.Status, true)))
            .ForMember(destination => destination.Role, options => options.MapFrom(source => Enum.Parse<UserRole>(source.Role, true)));
    }
}
