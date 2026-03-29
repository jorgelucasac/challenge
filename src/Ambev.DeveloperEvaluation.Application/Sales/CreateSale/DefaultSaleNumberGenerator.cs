namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class DefaultSaleNumberGenerator : ISaleNumberGenerator
{
    public string Generate()
    {
        return Guid.NewGuid().ToString("N");
    }
}