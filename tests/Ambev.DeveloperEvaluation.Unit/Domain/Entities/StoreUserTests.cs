using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events;
using FluentAssertions;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class StoreUserTests
{
    [Fact]
    public void Create_ValidData_CreatesUserAndAddsEvent()
    {
        var user = StoreUser.Create(
            "john@example.com",
            "john",
            "secret",
            "John",
            "Doe",
            "Sao Paulo",
            "Main Street",
            100,
            "01000-000",
            "-23.5",
            "-46.6",
            "11999999999",
            UserStatus.Active,
            UserRole.Customer);

        user.Email.Should().Be("john@example.com");
        user.Name.Firstname.Should().Be("John");
        user.Address.Geolocation.Lat.Should().Be("-23.5");
        user.DomainEvents.Should().ContainSingle().Which.Should().BeOfType<StoreUserCreatedEvent>();
    }

    [Fact]
    public void Update_ValidData_UpdatesUserAndAddsModifiedEvent()
    {
        var user = CreateUser();

        user.Update(
            "mary@example.com",
            "mary",
            "topsecret",
            "Mary",
            "Jane",
            "Rio",
            "Second Street",
            55,
            "22000-000",
            "-22.9",
            "-43.2",
            "21999999999",
            UserStatus.Inactive,
            UserRole.Manager);

        user.Email.Should().Be("mary@example.com");
        user.Status.Should().Be(UserStatus.Inactive);
        user.Role.Should().Be(UserRole.Manager);
        user.UpdatedAt.Should().NotBeNull();
        user.DomainEvents.Should().Contain(item => item is StoreUserModifiedEvent);
    }

    [Fact]
    public void Create_InvalidData_ThrowsDomainException()
    {
        var act = () => StoreUser.Create(
            "",
            "john",
            "secret",
            "John",
            "Doe",
            "Sao Paulo",
            "Main Street",
            100,
            "01000-000",
            "-23.5",
            "-46.6",
            "11999999999",
            UserStatus.Active,
            UserRole.Customer);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void MarkAsDeleted_AddsDeletedEvent()
    {
        var user = CreateUser();
        SetStoreUserId(user, 42);

        user.MarkAsDeleted();

        user.DomainEvents
            .OfType<StoreUserDeletedEvent>()
            .Should()
            .ContainSingle(item => item.UserId == 42);
    }

    private static StoreUser CreateUser()
    {
        return StoreUser.Create(
            "john@example.com",
            "john",
            "secret",
            "John",
            "Doe",
            "Sao Paulo",
            "Main Street",
            100,
            "01000-000",
            "-23.5",
            "-46.6",
            "11999999999",
            UserStatus.Active,
            UserRole.Customer);
    }

    private static void SetStoreUserId(StoreUser user, int id)
    {
        typeof(StoreUser).GetProperty(nameof(StoreUser.Id))!.SetValue(user, id);
    }
}
