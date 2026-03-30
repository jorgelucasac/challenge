namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts;

public class CartResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public List<CartProductResponse> Products { get; set; } = [];
}
