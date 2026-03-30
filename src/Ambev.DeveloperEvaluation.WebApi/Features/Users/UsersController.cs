using Ambev.DeveloperEvaluation.Application.StoreUsers.Common;
using Ambev.DeveloperEvaluation.Application.StoreUsers.CreateStoreUser;
using Ambev.DeveloperEvaluation.Application.StoreUsers.DeleteStoreUser;
using Ambev.DeveloperEvaluation.Application.StoreUsers.GetStoreUser;
using Ambev.DeveloperEvaluation.Application.StoreUsers.ListStoreUsers;
using Ambev.DeveloperEvaluation.Application.StoreUsers.UpdateStoreUser;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateStoreUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.DeleteStoreUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.GetStoreUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.ListStoreUsers;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateStoreUser;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users;

[ApiController]
[Route("api/[controller]")]
public class UsersController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UsersController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedStoreUsersResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ListUsers([FromQuery] ListStoreUsersRequest request, CancellationToken cancellationToken)
    {
        var validator = new ListStoreUsersRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var command = _mapper.Map<ListStoreUsersCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);

        return new OkObjectResult(new PagedStoreUsersResponse
        {
            Data = _mapper.Map<List<StoreUserResponse>>(result.Items),
            TotalItems = result.TotalCount,
            CurrentPage = result.CurrentPage,
            TotalPages = (int)Math.Ceiling(result.TotalCount / (double)result.PageSize)
        });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(StoreUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser([FromRoute] int id, CancellationToken cancellationToken)
    {
        var request = new GetStoreUserRequest { Id = id };
        var validator = new GetStoreUserRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var result = await _mediator.Send(new GetStoreUserCommand(id), cancellationToken);
        return new OkObjectResult(_mapper.Map<StoreUserResponse>(result));
    }

    [HttpPost]
    [ProducesResponseType(typeof(StoreUserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] CreateStoreUserRequest request, CancellationToken cancellationToken)
    {
        var validator = new CreateStoreUserRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var command = _mapper.Map<CreateStoreUserCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetUser), new { id = result.Id }, _mapper.Map<StoreUserResponse>(result));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(StoreUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody] UpdateStoreUserRequest request, CancellationToken cancellationToken)
    {
        request.Id = id;

        var validator = new UpdateStoreUserRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var command = _mapper.Map<UpdateStoreUserCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);
        return new OkObjectResult(_mapper.Map<StoreUserResponse>(result));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(StoreUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser([FromRoute] int id, CancellationToken cancellationToken)
    {
        var request = new DeleteStoreUserRequest { Id = id };
        var validator = new DeleteStoreUserRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var result = await _mediator.Send(new DeleteStoreUserCommand(id), cancellationToken);
        return new OkObjectResult(_mapper.Map<StoreUserResponse>(result));
    }
}
