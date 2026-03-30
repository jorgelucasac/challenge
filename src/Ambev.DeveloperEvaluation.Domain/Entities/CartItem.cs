namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class CartItem
{
    public int Id { get; private set; }
    public int CartId { get; private set; }
    public int ProductId { get; private set; }
    public int Quantity { get; private set; }

    private CartItem()
    {
    }

    public CartItem(int productId, int quantity)
    {
        Update(productId, quantity);
    }

    public void Update(int productId, int quantity)
    {
        if (productId <= 0)
        {
            throw new DomainException("Cart item product id must be greater than zero.");
        }

        if (quantity <= 0)
        {
            throw new DomainException("Cart item quantity must be greater than zero.");
        }

        ProductId = productId;
        Quantity = quantity;
    }
}
