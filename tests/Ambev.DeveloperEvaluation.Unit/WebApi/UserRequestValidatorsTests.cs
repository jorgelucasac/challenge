using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateStoreUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.DeleteStoreUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.GetStoreUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.ListStoreUsers;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateStoreUser;
using FluentAssertions;

namespace Ambev.DeveloperEvaluation.Unit.WebApi;

public class UserRequestValidatorsTests
{
    [Fact]
    public async Task CreateStoreUserRequestValidator_ShouldRejectInvalidRole()
    {
        var validator = new CreateStoreUserRequestValidator();
        var result = await validator.ValidateAsync(new CreateStoreUserRequest
        {
            Email = "john@example.com",
            Username = "john",
            Password = "secret",
            Name = new CreateStoreUserNameRequest { Firstname = "John", Lastname = "Doe" },
            Address = new CreateStoreUserAddressRequest
            {
                City = "Sao Paulo",
                Street = "Main Street",
                Number = 10,
                Zipcode = "01000-000",
                Geolocation = new CreateStoreUserGeolocationRequest { Lat = "-23.5", Long = "-46.6" }
            },
            Phone = "11999999999",
            Status = "Active",
            Role = "Invalid"
        });

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task GetStoreUserRequestValidator_ShouldRejectInvalidId()
    {
        var validator = new GetStoreUserRequestValidator();
        var result = await validator.ValidateAsync(new GetStoreUserRequest { Id = 0 });

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task ListStoreUsersRequestValidator_ShouldRejectInvalidPageSize()
    {
        var validator = new ListStoreUsersRequestValidator();
        var result = await validator.ValidateAsync(new ListStoreUsersRequest { _size = 0 });

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateStoreUserRequestValidator_ShouldRejectInvalidStatus()
    {
        var validator = new UpdateStoreUserRequestValidator();
        var result = await validator.ValidateAsync(new UpdateStoreUserRequest
        {
            Id = 1,
            Email = "mary@example.com",
            Username = "mary",
            Password = "secret",
            Name = new UpdateStoreUserNameRequest { Firstname = "Mary", Lastname = "Jane" },
            Address = new UpdateStoreUserAddressRequest
            {
                City = "Rio",
                Street = "Second Street",
                Number = 20,
                Zipcode = "22000-000",
                Geolocation = new UpdateStoreUserGeolocationRequest { Lat = "-22.9", Long = "-43.2" }
            },
            Phone = "21999999999",
            Status = "Whatever",
            Role = "Manager"
        });

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteStoreUserRequestValidator_ShouldRejectInvalidId()
    {
        var validator = new DeleteStoreUserRequestValidator();
        var result = await validator.ValidateAsync(new DeleteStoreUserRequest { Id = 0 });

        result.IsValid.Should().BeFalse();
    }
}
