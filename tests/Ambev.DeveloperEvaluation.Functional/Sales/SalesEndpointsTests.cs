using System.Net;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using FluentAssertions;
using Ambev.DeveloperEvaluation.Functional.Infrastructure;

namespace Ambev.DeveloperEvaluation.Functional.Sales;

[Collection(FunctionalApiCollection.CollectionName)]
public sealed class SalesEndpointsTests : IAsyncLifetime
{
    private readonly FunctionalApiFactory _factory;
    private readonly HttpClient _client;

    public SalesEndpointsTests(FunctionalApiFactory factory)
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
    public async Task CreateSale_ShouldReturnCreatedSalePayload()
    {
        var request = new CreateSaleRequest
        {
            SaleDate = CreateUtcDate(2026, 3, 29),
            CustomerExternalId = "customer-http-1",
            CustomerName = "HTTP Customer",
            BranchExternalId = "branch-http-1",
            BranchName = "HTTP Branch",
            Items =
            [
                new CreateSaleItemRequest
                {
                    ProductExternalId = "product-http-1",
                    ProductName = "HTTP Product One",
                    Quantity = 2,
                    UnitPrice = 10m
                },
                new CreateSaleItemRequest
                {
                    ProductExternalId = "product-http-2",
                    ProductName = "HTTP Product Two",
                    Quantity = 4,
                    UnitPrice = 5m
                }
            ]
        };

        var response = await _client.PostAsJsonAsync("/api/sales", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdPayload = await response.Content.ReadFromJsonAsync<ApiResponseWithData<CreateSaleResponse>>();
        createdPayload.Should().NotBeNull();
        createdPayload!.Success.Should().BeTrue();
        createdPayload.Data.Should().NotBeNull();
        createdPayload.Data!.SaleDate.Should().Be(CreateUtcDate(2026, 3, 29));
        createdPayload.Data.TotalAmount.Should().Be(38m);
        createdPayload.Data.Items.Should().HaveCount(2);
        createdPayload.Data.SaleNumber.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task GetSale_ShouldReturnDetailedPayloadForExistingSale()
    {
        var createdSale = await CreateSaleAsync(
            CreateUtcDate(2026, 3, 29),
            "HTTP Customer",
            "HTTP Branch",
            "product-http-1",
            4,
            5m);

        var getResponse = await _client.GetAsync($"/api/sales/{createdSale.Id}");

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var getPayload = await getResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateSaleResponse>>();
        getPayload.Should().NotBeNull();
        getPayload!.Success.Should().BeTrue();
        getPayload.Data.Should().NotBeNull();
        getPayload.Data!.Id.Should().Be(createdSale.Id);
        getPayload.Data.SaleNumber.Should().NotBeNullOrWhiteSpace();
        getPayload.Data.TotalAmount.Should().Be(18m);
        getPayload.Data.Items.Should().ContainSingle();
        getPayload.Data.Items.Single().ProductExternalId.Should().Be("product-http-1");
    }

    [Fact]
    public async Task GetSale_ShouldReturnNotFoundForMissingSale()
    {
        var response = await _client.GetAsync($"/api/sales/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var payload = await response.Content.ReadFromJsonAsync<ApiResponse>();
        payload.Should().NotBeNull();
        payload!.Success.Should().BeFalse();
        payload.Message.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task ListSales_ShouldReturnPaginatedSummaryFromEndpoint()
    {
        var firstSale = await CreateSaleAsync(
            CreateUtcDate(2026, 3, 28),
            "Acme Customer",
            "Rio",
            "product-list-1",
            10,
            10m);

        await CreateSaleAsync(
            CreateUtcDate(2026, 3, 29),
            "Acme Customer",
            "Sao Paulo",
            "product-list-2",
            4,
            10m);

        await _client.PostAsync($"/api/sales/{firstSale.Id}/cancel", content: null);

        var response = await _client.GetAsync("/api/sales?_page=1&_size=10&_order=saleDate_desc&customerName=Acme&isCancelled=true");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await response.Content.ReadFromJsonAsync<PaginatedResponse<ListSaleResponse>>();
        payload.Should().NotBeNull();
        payload!.Success.Should().BeTrue();
        payload.TotalCount.Should().Be(1);
        payload.Data.Should().NotBeNull();
        var sales = payload.Data!.ToList();
        sales.Should().ContainSingle();
        sales.Single().Id.Should().Be(firstSale.Id);
        sales.Single().IsCancelled.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateSale_ShouldReturnUpdatedSalePayload()
    {
        var createdSale = await CreateSaleAsync(
            CreateUtcDate(2026, 3, 29),
            "Lifecycle Customer",
            "Lifecycle Branch",
            "product-life-1",
            2,
            10m);

        var updateRequest = new UpdateSaleRequest
        {
            SaleDate = CreateUtcDate(2026, 3, 30),
            CustomerExternalId = "customer-life-updated",
            CustomerName = "Lifecycle Customer Updated",
            BranchExternalId = "branch-life-updated",
            BranchName = "Lifecycle Branch Updated",
            Items =
            [
                new UpdateSaleItemRequest
                {
                    Id = createdSale.Items.Single().Id,
                    ProductExternalId = "product-life-1",
                    ProductName = "Lifecycle Product Updated",
                    Quantity = 10,
                    UnitPrice = 12m,
                    IsCancelled = false
                }
            ]
        };

        var updateResponse = await _client.PutAsJsonAsync($"/api/sales/{createdSale.Id}", updateRequest);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var updatedPayload = await updateResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateSaleResponse>>();
        updatedPayload.Should().NotBeNull();
        updatedPayload!.Success.Should().BeTrue();
        updatedPayload.Data.Should().NotBeNull();
        updatedPayload.Data!.CustomerName.Should().Be("Lifecycle Customer Updated");
        updatedPayload.Data.BranchName.Should().Be("Lifecycle Branch Updated");
        updatedPayload.Data.SaleDate.Should().Be(CreateUtcDate(2026, 3, 30));
        updatedPayload.Data.TotalAmount.Should().Be(96m);
        updatedPayload.Data.Items.Should().ContainSingle();
        updatedPayload.Data.Items.Single().ProductName.Should().Be("Lifecycle Product Updated");
    }

    [Fact]
    public async Task CancelSale_ShouldReturnCancelledSale()
    {
        var createdSale = await CreateSaleAsync(
            CreateUtcDate(2026, 3, 29),
            "Cancel Customer",
            "Cancel Branch",
            "product-cancel-1",
            2,
            10m);

        var cancelResponse = await _client.PostAsync($"/api/sales/{createdSale.Id}/cancel", content: null);
        cancelResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var cancelledPayload = await cancelResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateSaleResponse>>();
        cancelledPayload.Should().NotBeNull();
        cancelledPayload!.Success.Should().BeTrue();
        cancelledPayload!.Data!.IsCancelled.Should().BeTrue();
        cancelledPayload.Data.TotalAmount.Should().Be(0m);
        cancelledPayload.Data.Items.Should().OnlyContain(item => item.IsCancelled);
    }

    [Fact]
    public async Task ActivateSale_ShouldReturnReactivatedSale()
    {
        var createdSale = await CreateSaleAsync(
            CreateUtcDate(2026, 3, 29),
            "Activate Customer",
            "Activate Branch",
            "product-activate-1",
            10,
            12m);

        await _client.PostAsync($"/api/sales/{createdSale.Id}/cancel", content: null);

        var activateResponse = await _client.PostAsync($"/api/sales/{createdSale.Id}/activate", content: null);
        activateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var activatedPayload = await activateResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateSaleResponse>>();
        activatedPayload.Should().NotBeNull();
        activatedPayload!.Success.Should().BeTrue();
        activatedPayload!.Data!.IsCancelled.Should().BeFalse();
        activatedPayload.Data.TotalAmount.Should().Be(96m);
        activatedPayload.Data.Items.Should().OnlyContain(item => !item.IsCancelled);
    }

    [Fact]
    public async Task DeleteSale_ShouldRemoveSaleAndReturnNotFoundAfterwards()
    {
        var createdSale = await CreateSaleAsync(
            CreateUtcDate(2026, 3, 29),
            "Delete Customer",
            "Delete Branch",
            "product-delete-1",
            2,
            10m);

        var deleteResponse = await _client.DeleteAsync($"/api/sales/{createdSale.Id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var deletePayload = await deleteResponse.Content.ReadFromJsonAsync<ApiResponse>();
        deletePayload.Should().NotBeNull();
        deletePayload!.Success.Should().BeTrue();

        var getAfterDeleteResponse = await _client.GetAsync($"/api/sales/{createdSale.Id}");
        getAfterDeleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var notFoundPayload = await getAfterDeleteResponse.Content.ReadFromJsonAsync<ApiResponse>();
        notFoundPayload.Should().NotBeNull();
        notFoundPayload!.Success.Should().BeFalse();
    }

    private async Task<CreateSaleResponse> CreateSaleAsync(
        DateTime saleDate,
        string customerName,
        string branchName,
        string productExternalId,
        int quantity,
        decimal unitPrice)
    {
        var request = new CreateSaleRequest
        {
            SaleDate = saleDate,
            CustomerExternalId = $"customer-{Guid.NewGuid():N}",
            CustomerName = customerName,
            BranchExternalId = $"branch-{Guid.NewGuid():N}",
            BranchName = branchName,
            Items =
            [
                new CreateSaleItemRequest
                {
                    ProductExternalId = productExternalId,
                    ProductName = $"Product {productExternalId}",
                    Quantity = quantity,
                    UnitPrice = unitPrice
                }
            ]
        };

        var response = await _client.PostAsJsonAsync("/api/sales", request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<ApiResponseWithData<CreateSaleResponse>>();
        payload.Should().NotBeNull();
        payload!.Data.Should().NotBeNull();
        return payload.Data!;
    }

    private static DateTime CreateUtcDate(int year, int month, int day)
    {
        return new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
    }
}
