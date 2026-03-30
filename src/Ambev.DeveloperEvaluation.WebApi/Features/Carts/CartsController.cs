using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;
using Ambev.DeveloperEvaluation.Application.Carts.GetCart;
using Ambev.DeveloperEvaluation.Application.Carts.ListCarts;
using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.DeleteCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.ListCarts;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts;

[ApiController]
[Route("api/[controller]")]
public class CartsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CartsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedCartsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ListCarts([FromQuery] ListCartsRequest request, CancellationToken cancellationToken)
    {
        var validator = new ListCartsRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var command = _mapper.Map<ListCartsCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);

        return new OkObjectResult(new PagedCartsResponse
        {
            Data = _mapper.Map<List<CartResponse>>(result.Items),
            TotalItems = result.TotalCount,
            CurrentPage = result.CurrentPage,
            TotalPages = (int)Math.Ceiling(result.TotalCount / (double)result.PageSize)
        });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCart([FromRoute] int id, CancellationToken cancellationToken)
    {
        var request = new GetCartRequest { Id = id };
        var validator = new GetCartRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var result = await _mediator.Send(new GetCartCommand(id), cancellationToken);
        return new OkObjectResult(_mapper.Map<CartResponse>(result));
    }

    [HttpPost]
    [ProducesResponseType(typeof(CartResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCart([FromBody] CreateCartRequest request, CancellationToken cancellationToken)
    {
        var validator = new CreateCartRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var command = _mapper.Map<CreateCartCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetCart), new { id = result.Id }, _mapper.Map<CartResponse>(result));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCart([FromRoute] int id, [FromBody] UpdateCartRequest request, CancellationToken cancellationToken)
    {
        request.Id = id;

        var validator = new UpdateCartRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var command = _mapper.Map<UpdateCartCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);
        return new OkObjectResult(_mapper.Map<CartResponse>(result));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(DeleteCartResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCart([FromRoute] int id, CancellationToken cancellationToken)
    {
        var request = new DeleteCartRequest { Id = id };
        var validator = new DeleteCartRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        await _mediator.Send(new DeleteCartCommand(id), cancellationToken);
        return new OkObjectResult(new DeleteCartResponse { Message = "Cart deleted successfully" });
    }
}
