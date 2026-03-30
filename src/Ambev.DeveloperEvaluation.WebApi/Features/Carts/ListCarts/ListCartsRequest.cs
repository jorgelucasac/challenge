namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.ListCarts;

public class ListCartsRequest
{
    public int _page { get; set; } = 1;
    public int _size { get; set; } = 10;
    public string _order { get; set; } = "id asc";
}
