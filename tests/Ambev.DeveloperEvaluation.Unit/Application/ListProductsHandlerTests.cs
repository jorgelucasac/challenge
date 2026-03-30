using Ambev.DeveloperEvaluation.Application.Products.Common;
using Ambev.DeveloperEvaluation.Application.Products.ListProducts;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class ListProductsHandlerTests
{
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();

    [Fact(DisplayName = "Given filters When listing products Then returns paged result")]
    public async Task Handle_ValidRequest_ReturnsPagedProducts()
    {
        var handler = new ListProductsHandler(_productRepository, _mapper);
        var products = new List<Product>
        {
            Product.Create("Backpack", 109.95m, "Travel backpack", "bags", "https://image.test/backpack.png", 4.5m, 12)
        };
        var resultItems = new List<ProductResult>
        {
            new() { Id = 1, Title = "Backpack" }
        };

        _productRepository.ListAsync(Arg.Any<ProductListFilter>(), Arg.Any<CancellationToken>())
            .Returns(new PagedResult<Product>(products, 2, 5, 11));
        _mapper.Map<List<ProductResult>>(Arg.Any<List<Product>>()).Returns(resultItems);

        var response = await handler.Handle(new ListProductsCommand
        {
            Page = 2,
            Size = 5,
            Order = "price desc, title asc",
            Title = "Back*"
        }, CancellationToken.None);

        response.CurrentPage.Should().Be(2);
        response.TotalCount.Should().Be(11);
        response.Items.Should().ContainSingle();
    }
}
