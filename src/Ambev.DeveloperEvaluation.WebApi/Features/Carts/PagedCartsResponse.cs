namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts;

public class PagedCartsResponse
{
    public List<CartResponse> Data { get; set; } = [];
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}
