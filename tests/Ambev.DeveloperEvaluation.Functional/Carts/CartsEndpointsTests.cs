using System.Net;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Functional.Infrastructure;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;
using FluentAssertions;

namespace Ambev.DeveloperEvaluation.Functional.Carts;

[Collection(FunctionalApiCollection.CollectionName)]
public sealed class CartsEndpointsTests : IAsyncLifetime
{
    private readonly FunctionalApiFactory _factory;
    private readonly HttpClient _client;

    public CartsEndpointsTests(FunctionalApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateApiClient();
    }

    public Task InitializeAsync() => _factory.ResetDatabaseAsync();

    public Task DisposeAsync()
    {
        _client.Dispose();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task CreateCart_ShouldReturnCreatedPayload()
    {
        var response = await _client.PostAsJsonAsync("/api/carts", BuildCreateRequest(1));

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var payload = await response.Content.ReadFromJsonAsync<CartResponse>();
        payload.Should().NotBeNull();
        payload!.Id.Should().BeGreaterThan(0);
        payload.Products.Should().ContainSingle();
    }

    [Fact]
    public async Task GetCart_ShouldReturnCartForExistingId()
    {
        var created = await CreateCartAsync(1);

        var response = await _client.GetAsync($"/api/carts/{created.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await response.Content.ReadFromJsonAsync<CartResponse>();
        payload!.Id.Should().Be(created.Id);
        payload.UserId.Should().Be(1);
    }

    [Fact]
    public async Task GetCart_ShouldReturnNotFoundForMissingId()
    {
        var response = await _client.GetAsync("/api/carts/999999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ListCarts_ShouldApplyPaginationAndOrdering()
    {
        await CreateCartAsync(2);
        await CreateCartAsync(1);

        var order = Uri.EscapeDataString("userId asc, id desc");
        var response = await _client.GetAsync($"/api/carts?_page=1&_size=10&_order={order}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await response.Content.ReadFromJsonAsync<PagedCartsResponse>();
        payload.Should().NotBeNull();
        payload!.TotalItems.Should().Be(2);
        payload.Data[0].UserId.Should().Be(1);
        payload.Data[1].UserId.Should().Be(2);
    }

    [Fact]
    public async Task UpdateCart_ShouldReturnUpdatedPayload()
    {
        var created = await CreateCartAsync(1);
        var request = new UpdateCartRequest
        {
            UserId = 3,
            Date = new DateTime(2026, 3, 31, 0, 0, 0, DateTimeKind.Utc),
            Products =
            [
                new UpdateCartProductRequest
                {
                    ProductId = 20,
                    Quantity = 5
                }
            ]
        };

        var response = await _client.PutAsJsonAsync($"/api/carts/{created.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await response.Content.ReadFromJsonAsync<CartResponse>();
        payload!.UserId.Should().Be(3);
        payload.Products.Should().ContainSingle(item => item.ProductId == 20 && item.Quantity == 5);
    }

    [Fact]
    public async Task DeleteCart_ShouldRemoveCart()
    {
        var created = await CreateCartAsync(1);

        var deleteResponse = await _client.DeleteAsync($"/api/carts/{created.Id}");

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await deleteResponse.Content.ReadFromJsonAsync<DeleteCartResponse>();
        payload!.Message.Should().Be("Cart deleted successfully");

        var getResponse = await _client.GetAsync($"/api/carts/{created.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private async Task<CartResponse> CreateCartAsync(int userId)
    {
        var response = await _client.PostAsJsonAsync("/api/carts", BuildCreateRequest(userId));
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<CartResponse>())!;
    }

    private static CreateCartRequest BuildCreateRequest(int userId)
    {
        return new CreateCartRequest
        {
            UserId = userId,
            Date = new DateTime(2026, 3, 30, 0, 0, 0, DateTimeKind.Utc),
            Products =
            [
                new CreateCartProductRequest
                {
                    ProductId = 10,
                    Quantity = 2
                }
            ]
        };
    }
}
