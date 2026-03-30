using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProductCategories;

public class ListProductCategoriesHandler : IRequestHandler<ListProductCategoriesQuery, IReadOnlyList<string>>
{
    private readonly IProductRepository _productRepository;

    public ListProductCategoriesHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task<IReadOnlyList<string>> Handle(ListProductCategoriesQuery request, CancellationToken cancellationToken)
    {
        return _productRepository.ListCategoriesAsync(cancellationToken);
    }
}
