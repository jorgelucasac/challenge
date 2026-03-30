using Ambev.DeveloperEvaluation.Application.StoreUsers.CreateStoreUser;
using Ambev.DeveloperEvaluation.Application.StoreUsers.DeleteStoreUser;
using Ambev.DeveloperEvaluation.Application.StoreUsers.GetStoreUser;
using Ambev.DeveloperEvaluation.Application.StoreUsers.ListStoreUsers;
using Ambev.DeveloperEvaluation.Application.StoreUsers.UpdateStoreUser;
using Ambev.DeveloperEvaluation.Domain.Enums;
using FluentAssertions;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class StoreUserValidatorsTests
{
    [Fact]
    public void CreateStoreUserValidator_ShouldRejectEmptyEmail()
    {
        var validator = new CreateStoreUserValidator();
        var result = validator.Validate(new CreateStoreUserCommand());

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void GetStoreUserValidator_ShouldRejectInvalidId()
    {
        var validator = new GetStoreUserValidator();
        var result = validator.Validate(new GetStoreUserCommand(0));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void ListStoreUsersValidator_ShouldRejectInvalidOrder()
    {
        var validator = new ListStoreUsersValidator();
        var result = validator.Validate(new ListStoreUsersCommand { Order = "createdAt desc" });

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void UpdateStoreUserValidator_ShouldRejectInvalidId()
    {
        var validator = new UpdateStoreUserValidator();
        var result = validator.Validate(new UpdateStoreUserCommand
        {
            Email = "john@example.com",
            Username = "john",
            Password = "secret",
            Firstname = "John",
            Lastname = "Doe",
            City = "Sao Paulo",
            Street = "Main Street",
            Number = 10,
            Zipcode = "01000-000",
            Lat = "-23.5",
            Long = "-46.6",
            Phone = "11999999999",
            Status = UserStatus.Active,
            Role = UserRole.Customer
        });

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void DeleteStoreUserValidator_ShouldRejectInvalidId()
    {
        var validator = new DeleteStoreUserValidator();
        var result = validator.Validate(new DeleteStoreUserCommand(0));

        result.IsValid.Should().BeFalse();
    }
}
