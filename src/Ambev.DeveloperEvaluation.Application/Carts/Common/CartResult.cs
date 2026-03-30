namespace Ambev.DeveloperEvaluation.Application.Carts.Common;

public class CartResult
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public List<CartProductResult> Products { get; set; } = [];
}
