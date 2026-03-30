using Ambev.DeveloperEvaluation.Application.Products.Common;
using Ambev.DeveloperEvaluation.Domain.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProducts;

public class ListProductsCommand : IRequest<PagedResult<ProductResult>>
{
    public int Page { get; set; } = ListProductsDefaults.DefaultPage;
    public int Size { get; set; } = ListProductsDefaults.DefaultPageSize;
    public string Order { get; set; } = ListProductsDefaults.DefaultOrder;
    public string? Title { get; set; }
    public string? Category { get; set; }
    public decimal? Price { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}
