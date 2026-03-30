using Ambev.DeveloperEvaluation.Application.StoreUsers.Common;
using Ambev.DeveloperEvaluation.Application.StoreUsers.CreateStoreUser;
using Ambev.DeveloperEvaluation.Application.StoreUsers.DeleteStoreUser;
using Ambev.DeveloperEvaluation.Application.StoreUsers.GetStoreUser;
using Ambev.DeveloperEvaluation.Application.StoreUsers.ListStoreUsers;
using Ambev.DeveloperEvaluation.Application.StoreUsers.UpdateStoreUser;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class StoreUserHandlersTests
{
    private readonly IStoreUserRepository _repository = Substitute.For<IStoreUserRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();

    [Fact]
    public async Task CreateStoreUser_ShouldPersistAndCommit()
    {
        var handler = new CreateStoreUserHandler(_repository, _unitOfWork, _mapper);
        var command = BuildCreateCommand();
        var result = new StoreUserResult { Id = 1, Email = command.Email };

        _mapper.Map<StoreUserResult>(Arg.Any<StoreUser>()).Returns(result);

        var response = await handler.Handle(command, CancellationToken.None);

        response.Email.Should().Be(command.Email);
        await _repository.Received(1).CreateAsync(Arg.Any<StoreUser>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetStoreUser_ShouldReturnMappedUser()
    {
        var handler = new GetStoreUserHandler(_repository, _mapper);
        var user = BuildUser();
        var result = new StoreUserResult { Id = 1, Email = user.Email };

        _repository.GetByIdAsync(1, Arg.Any<CancellationToken>()).Returns(user);
        _mapper.Map<StoreUserResult>(user).Returns(result);

        var response = await handler.Handle(new GetStoreUserCommand(1), CancellationToken.None);

        response.Id.Should().Be(1);
    }

    [Fact]
    public async Task ListStoreUsers_ShouldReturnPagedResult()
    {
        var handler = new ListStoreUsersHandler(_repository, _mapper);
        var users = new List<StoreUser> { BuildUser() };
        var mapped = new List<StoreUserResult> { new() { Id = 1, Email = "john@example.com" } };

        _repository.ListAsync(Arg.Any<StoreUserListFilter>(), Arg.Any<CancellationToken>())
            .Returns(new PagedResult<StoreUser>(users, 1, 10, 1));
        _mapper.Map<List<StoreUserResult>>(users).Returns(mapped);

        var response = await handler.Handle(new ListStoreUsersCommand(), CancellationToken.None);

        response.TotalCount.Should().Be(1);
        response.Items.Should().ContainSingle();
    }

    [Fact]
    public async Task UpdateStoreUser_ShouldUpdateAndCommit()
    {
        var handler = new UpdateStoreUserHandler(_repository, _unitOfWork, _mapper);
        var user = BuildUser();
        var command = new UpdateStoreUserCommand
        {
            Id = 1,
            Email = "mary@example.com",
            Username = "mary",
            Password = "secret",
            Firstname = "Mary",
            Lastname = "Jane",
            City = "Rio",
            Street = "Second Street",
            Number = 20,
            Zipcode = "22000-000",
            Lat = "-22.9",
            Long = "-43.2",
            Phone = "21999999999",
            Status = UserStatus.Inactive,
            Role = UserRole.Manager
        };
        var result = new StoreUserResult { Id = 1, Email = command.Email };

        _repository.GetByIdForUpdateAsync(1, Arg.Any<CancellationToken>()).Returns(user);
        _mapper.Map<StoreUserResult>(user).Returns(result);

        var response = await handler.Handle(command, CancellationToken.None);

        response.Email.Should().Be("mary@example.com");
        await _repository.Received(1).UpdateAsync(user, Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteStoreUser_ShouldDeleteAndCommit()
    {
        var handler = new DeleteStoreUserHandler(_repository, _unitOfWork, _mapper);
        var user = BuildUser();
        var result = new StoreUserResult { Id = 1, Email = user.Email };

        _repository.GetByIdForUpdateAsync(1, Arg.Any<CancellationToken>()).Returns(user);
        _repository.DeleteAsync(1, Arg.Any<CancellationToken>()).Returns(true);
        _mapper.Map<StoreUserResult>(user).Returns(result);

        var response = await handler.Handle(new DeleteStoreUserCommand(1), CancellationToken.None);

        response.Id.Should().Be(1);
        await _repository.Received(1).DeleteAsync(1, Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    private static CreateStoreUserCommand BuildCreateCommand()
    {
        return new CreateStoreUserCommand
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
        };
    }

    private static StoreUser BuildUser()
    {
        var user = StoreUser.Create(
            "john@example.com",
            "john",
            "secret",
            "John",
            "Doe",
            "Sao Paulo",
            "Main Street",
            10,
            "01000-000",
            "-23.5",
            "-46.6",
            "11999999999",
            UserStatus.Active,
            UserRole.Customer);

        typeof(StoreUser).GetProperty(nameof(StoreUser.Id))!.SetValue(user, 1);
        return user;
    }
}
