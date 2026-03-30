using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProductCategories;

public record ListProductCategoriesQuery : IRequest<IReadOnlyList<string>>;
