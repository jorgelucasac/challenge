using Ambev.DeveloperEvaluation.Application.Products.Common;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProducts;

public class ListProductsHandler : IRequestHandler<ListProductsCommand, PagedResult<ProductResult>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ListProductsHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<PagedResult<ProductResult>> Handle(ListProductsCommand request, CancellationToken cancellationToken)
    {
        var filter = new ProductListFilter
        {
            Page = request.Page,
            Size = request.Size,
            Order = ListProductsOrderParser.Parse(request.Order),
            Title = request.Title?.Trim(),
            Category = request.Category?.Trim(),
            ExactPrice = request.Price,
            MinPrice = request.MinPrice,
            MaxPrice = request.MaxPrice
        };

        var pagedProducts = await _productRepository.ListAsync(filter, cancellationToken);
        var items = _mapper.Map<List<ProductResult>>(pagedProducts.Items);

        return new PagedResult<ProductResult>(items, pagedProducts.CurrentPage, pagedProducts.PageSize, pagedProducts.TotalCount);
    }
}
