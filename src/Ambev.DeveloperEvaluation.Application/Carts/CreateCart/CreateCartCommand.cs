using Ambev.DeveloperEvaluation.Application.Carts.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

public class CreateCartCommand : IRequest<CartResult>
{
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public List<CreateCartProductInput> Products { get; set; } = [];
}
