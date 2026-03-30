using System.Net;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Functional.Infrastructure;
using Ambev.DeveloperEvaluation.WebApi.Features.Products;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;
using FluentAssertions;

namespace Ambev.DeveloperEvaluation.Functional.Products;

[Collection(FunctionalApiCollection.CollectionName)]
public sealed class ProductsEndpointsTests : IAsyncLifetime
{
    private readonly FunctionalApiFactory _factory;
    private readonly HttpClient _client;

    public ProductsEndpointsTests(FunctionalApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateApiClient();
    }

    public Task InitializeAsync()
    {
        return _factory.ResetDatabaseAsync();
    }

    public Task DisposeAsync()
    {
        _client.Dispose();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task CreateProduct_ShouldReturnCreatedProductPayload()
    {
        var request = BuildCreateRequest("Backpack", 109.95m, "bags");

        var response = await _client.PostAsJsonAsync("/api/products", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var payload = await response.Content.ReadFromJsonAsync<ProductResponse>();
        payload.Should().NotBeNull();
        payload!.Id.Should().BeGreaterThan(0);
        payload.Title.Should().Be("Backpack");
        payload.Rating.Rate.Should().Be(4.5m);
    }

    [Fact]
    public async Task GetProduct_ShouldReturnProductForExistingId()
    {
        var createdProduct = await CreateProductAsync("Backpack", 109.95m, "bags");

        var response = await _client.GetAsync($"/api/products/{createdProduct.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await response.Content.ReadFromJsonAsync<ProductResponse>();
        payload.Should().NotBeNull();
        payload!.Id.Should().Be(createdProduct.Id);
        payload.Title.Should().Be("Backpack");
    }

    [Fact]
    public async Task GetProduct_ShouldReturnNotFoundForMissingId()
    {
        var response = await _client.GetAsync("/api/products/999999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ListProducts_ShouldApplyPaginationAndOrdering()
    {
        await CreateProductAsync("Alpha Jacket", 50m, "clothing");
        await CreateProductAsync("Zulu Jacket", 50m, "clothing");
        await CreateProductAsync("Backpack", 150m, "bags");

        var order = Uri.EscapeDataString("price desc, title asc");
        var response = await _client.GetAsync($"/api/products?_page=1&_size=2&_order={order}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await response.Content.ReadFromJsonAsync<PagedProductsResponse>();
        payload.Should().NotBeNull();
        payload!.TotalItems.Should().Be(3);
        payload.CurrentPage.Should().Be(1);
        payload.Data.Should().HaveCount(2);
        payload.Data[0].Title.Should().Be("Backpack");
        payload.Data[1].Title.Should().Be("Alpha Jacket");
    }

    [Fact]
    public async Task ListCategories_ShouldReturnDistinctOrderedCategories()
    {
        await CreateProductAsync("Backpack", 150m, "bags");
        await CreateProductAsync("Jacket", 50m, "clothing");
        await CreateProductAsync("Boots", 75m, "clothing");

        var response = await _client.GetAsync("/api/products/categories");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await response.Content.ReadFromJsonAsync<List<string>>();
        payload.Should().BeEquivalentTo(["bags", "clothing"], options => options.WithStrictOrdering());
    }

    [Fact]
    public async Task ListProductsByCategory_ShouldReturnFilteredProducts()
    {
        await CreateProductAsync("Backpack", 150m, "bags");
        await CreateProductAsync("Jacket", 50m, "clothing");

        var response = await _client.GetAsync("/api/products/category/clothing?_page=1&_size=10&_order=title%20asc");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await response.Content.ReadFromJsonAsync<PagedProductsResponse>();
        payload.Should().NotBeNull();
        payload!.TotalItems.Should().Be(1);
        payload.Data.Should().ContainSingle();
        payload.Data[0].Category.Should().Be("clothing");
    }

    [Fact]
    public async Task UpdateProduct_ShouldReturnUpdatedPayload()
    {
        var createdProduct = await CreateProductAsync("Backpack", 109.95m, "bags");
        var request = new UpdateProductRequest
        {
            Title = "Jacket",
            Price = 89.90m,
            Description = "Winter jacket",
            Category = "clothing",
            Image = "https://image.test/jacket.png",
            Rating = new UpdateProductRatingRequest
            {
                Rate = 4.9m,
                Count = 20
            }
        };

        var response = await _client.PutAsJsonAsync($"/api/products/{createdProduct.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await response.Content.ReadFromJsonAsync<ProductResponse>();
        payload.Should().NotBeNull();
        payload!.Title.Should().Be("Jacket");
        payload.Category.Should().Be("clothing");
    }

    [Fact]
    public async Task DeleteProduct_ShouldRemoveProduct()
    {
        var createdProduct = await CreateProductAsync("Backpack", 109.95m, "bags");

        var deleteResponse = await _client.DeleteAsync($"/api/products/{createdProduct.Id}");

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await deleteResponse.Content.ReadFromJsonAsync<DeleteProductResponse>();
        payload.Should().NotBeNull();
        payload!.Message.Should().Be("Product deleted successfully");

        var getResponse = await _client.GetAsync($"/api/products/{createdProduct.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private async Task<ProductResponse> CreateProductAsync(string title, decimal price, string category)
    {
        var response = await _client.PostAsJsonAsync("/api/products", BuildCreateRequest(title, price, category));
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<ProductResponse>();
        payload.Should().NotBeNull();
        return payload!;
    }

    private static CreateProductRequest BuildCreateRequest(string title, decimal price, string category)
    {
        return new CreateProductRequest
        {
            Title = title,
            Price = price,
            Description = $"{title} description",
            Category = category,
            Image = $"https://image.test/{title.Replace(' ', '-').ToLowerInvariant()}.png",
            Rating = new CreateProductRatingRequest
            {
                Rate = 4.5m,
                Count = 12
            }
        };
    }
}
