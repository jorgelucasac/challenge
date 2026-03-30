using Ambev.DeveloperEvaluation.Application.StoreUsers.Common;
using Ambev.DeveloperEvaluation.Application.StoreUsers.CreateStoreUser;
using Ambev.DeveloperEvaluation.Application.StoreUsers.DeleteStoreUser;
using Ambev.DeveloperEvaluation.Application.StoreUsers.GetStoreUser;
using Ambev.DeveloperEvaluation.Application.StoreUsers.ListStoreUsers;
using Ambev.DeveloperEvaluation.Application.StoreUsers.UpdateStoreUser;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Users;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateStoreUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.ListStoreUsers;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateStoreUser;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace Ambev.DeveloperEvaluation.Unit.WebApi;

public class UsersControllerTests
{
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _controller = new UsersController(_mediator, _mapper);
    }

    [Fact]
    public async Task CreateUser_ShouldReturnCreatedPayload()
    {
        var request = BuildCreateRequest();
        var command = new CreateStoreUserCommand();
        var result = new StoreUserResult { Id = 1, Email = request.Email };
        var response = new StoreUserResponse { Id = 1, Email = request.Email };

        _mapper.Map<CreateStoreUserCommand>(request).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>()).Returns(result);
        _mapper.Map<StoreUserResponse>(result).Returns(response);

        var actionResult = await _controller.CreateUser(request, CancellationToken.None);

        var created = actionResult.Should().BeOfType<CreatedAtActionResult>().Subject;
        created.Value.Should().Be(response);
    }

    [Fact]
    public async Task GetUser_ShouldReturnOkPayload()
    {
        var result = new StoreUserResult { Id = 1, Email = "john@example.com" };
        var response = new StoreUserResponse { Id = 1, Email = "john@example.com" };

        _mediator.Send(Arg.Any<GetStoreUserCommand>(), Arg.Any<CancellationToken>()).Returns(result);
        _mapper.Map<StoreUserResponse>(result).Returns(response);

        var actionResult = await _controller.GetUser(1, CancellationToken.None);

        var ok = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().Be(response);
    }

    [Fact]
    public async Task ListUsers_ShouldReturnPagedPayload()
    {
        var request = new ListStoreUsersRequest { _page = 1, _size = 10, _order = "username asc, email desc" };
        var command = new ListStoreUsersCommand();
        var result = new PagedResult<StoreUserResult>([new StoreUserResult { Id = 1, Email = "john@example.com" }], 1, 10, 1);
        var response = new List<StoreUserResponse> { new() { Id = 1, Email = "john@example.com" } };

        _mapper.Map<ListStoreUsersCommand>(request).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>()).Returns(result);
        _mapper.Map<List<StoreUserResponse>>(result.Items).Returns(response);

        var actionResult = await _controller.ListUsers(request, CancellationToken.None);

        var ok = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeOfType<PagedStoreUsersResponse>().Which.TotalItems.Should().Be(1);
    }

    [Fact]
    public async Task UpdateUser_ShouldReturnOkPayload()
    {
        var request = new UpdateStoreUserRequest
        {
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
            Status = "Inactive",
            Role = "Manager"
        };
        var command = new UpdateStoreUserCommand();
        var result = new StoreUserResult { Id = 1, Email = request.Email };
        var response = new StoreUserResponse { Id = 1, Email = request.Email };

        _mapper.Map<UpdateStoreUserCommand>(Arg.Any<UpdateStoreUserRequest>()).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>()).Returns(result);
        _mapper.Map<StoreUserResponse>(result).Returns(response);

        var actionResult = await _controller.UpdateUser(1, request, CancellationToken.None);

        var ok = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().Be(response);
    }

    [Fact]
    public async Task DeleteUser_ShouldReturnDeletedPayload()
    {
        var result = new StoreUserResult { Id = 1, Email = "john@example.com" };
        var response = new StoreUserResponse { Id = 1, Email = "john@example.com" };

        _mediator.Send(Arg.Any<DeleteStoreUserCommand>(), Arg.Any<CancellationToken>()).Returns(result);
        _mapper.Map<StoreUserResponse>(result).Returns(response);

        var actionResult = await _controller.DeleteUser(1, CancellationToken.None);

        var ok = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().Be(response);
    }

    private static CreateStoreUserRequest BuildCreateRequest()
    {
        return new CreateStoreUserRequest
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
            Role = "Customer"
        };
    }
}
