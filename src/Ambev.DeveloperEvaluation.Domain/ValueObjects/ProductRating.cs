namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public class ProductRating
{
    public decimal Rate { get; private set; }
    public int Count { get; private set; }

    private ProductRating()
    {
    }

    public ProductRating(decimal rate, int count)
    {
        Update(rate, count);
    }

    public void Update(decimal rate, int count)
    {
        if (rate < 0)
        {
            throw new DomainException("Product rating rate must be greater than or equal to zero.");
        }

        if (count < 0)
        {
            throw new DomainException("Product rating count must be greater than or equal to zero.");
        }

        Rate = rate;
        Count = count;
    }
}
