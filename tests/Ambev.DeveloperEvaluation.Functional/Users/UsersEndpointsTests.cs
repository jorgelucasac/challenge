using System.Net;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Functional.Infrastructure;
using Ambev.DeveloperEvaluation.WebApi.Features.Users;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateStoreUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateStoreUser;
using FluentAssertions;

namespace Ambev.DeveloperEvaluation.Functional.Users;

[Collection(FunctionalApiCollection.CollectionName)]
public sealed class UsersEndpointsTests : IAsyncLifetime
{
    private readonly FunctionalApiFactory _factory;
    private readonly HttpClient _client;

    public UsersEndpointsTests(FunctionalApiFactory factory)
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
    public async Task CreateUser_ShouldReturnCreatedPayload()
    {
        var response = await _client.PostAsJsonAsync("/api/users", BuildCreateRequest("john@example.com", "john"));

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var payload = await response.Content.ReadFromJsonAsync<StoreUserResponse>();
        payload.Should().NotBeNull();
        payload!.Id.Should().BeGreaterThan(0);
        payload.Status.Should().Be("Active");
        payload.Role.Should().Be("Customer");
    }

    [Fact]
    public async Task GetUser_ShouldReturnUserForExistingId()
    {
        var created = await CreateUserAsync("john@example.com", "john");

        var response = await _client.GetAsync($"/api/users/{created.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await response.Content.ReadFromJsonAsync<StoreUserResponse>();
        payload!.Id.Should().Be(created.Id);
        payload.Email.Should().Be(created.Email);
    }

    [Fact]
    public async Task GetUser_ShouldReturnNotFoundForMissingId()
    {
        var response = await _client.GetAsync("/api/users/999999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ListUsers_ShouldApplyPaginationAndOrdering()
    {
        await CreateUserAsync("bravo@example.com", "bravo");
        await CreateUserAsync("alpha@example.com", "alpha");

        var order = Uri.EscapeDataString("username asc, email desc");
        var response = await _client.GetAsync($"/api/users?_page=1&_size=10&_order={order}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await response.Content.ReadFromJsonAsync<PagedStoreUsersResponse>();
        payload.Should().NotBeNull();
        payload!.TotalItems.Should().Be(2);
        payload.Data[0].Username.Should().Be("alpha");
        payload.Data[1].Username.Should().Be("bravo");
    }

    [Fact]
    public async Task UpdateUser_ShouldReturnUpdatedPayload()
    {
        var created = await CreateUserAsync("john@example.com", "john");
        var request = new UpdateStoreUserRequest
        {
            Email = "mary@example.com",
            Username = "mary",
            Password = "secret2",
            Name = new UpdateStoreUserNameRequest { Firstname = "Mary", Lastname = "Jane" },
            Address = new UpdateStoreUserAddressRequest
            {
                City = "Rio",
                Street = "Second Street",
                Number = 20,
                Zipcode = "22000-000",
                Geolocation = new UpdateStoreUserGeolocationRequest { Lat = "-22.9", Long = "-43.2" }
            },
            Phone = "21999999999",
            Status = "Inactive",
            Role = "Manager"
        };

        var response = await _client.PutAsJsonAsync($"/api/users/{created.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await response.Content.ReadFromJsonAsync<StoreUserResponse>();
        payload!.Email.Should().Be("mary@example.com");
        payload.Role.Should().Be("Manager");
    }

    [Fact]
    public async Task DeleteUser_ShouldRemoveUser()
    {
        var created = await CreateUserAsync("john@example.com", "john");

        var deleteResponse = await _client.DeleteAsync($"/api/users/{created.Id}");

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await deleteResponse.Content.ReadFromJsonAsync<StoreUserResponse>();
        payload!.Id.Should().Be(created.Id);

        var getResponse = await _client.GetAsync($"/api/users/{created.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private async Task<StoreUserResponse> CreateUserAsync(string email, string username)
    {
        var response = await _client.PostAsJsonAsync("/api/users", BuildCreateRequest(email, username));
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<StoreUserResponse>())!;
    }

    private static CreateStoreUserRequest BuildCreateRequest(string email, string username)
    {
        return new CreateStoreUserRequest
        {
            Email = email,
            Username = username,
            Password = "secret",
            Name = new CreateStoreUserNameRequest { Firstname = "John", Lastname = "Doe" },
            Address = new CreateStoreUserAddressRequest
            {
                City = "Sao Paulo",
                Street = "Main Street",
                Number = 10,
                Zipcode = "01000-000",
                Geolocation = new CreateStoreUserGeolocationRequest { Lat = "-23.5", Long = "-46.6" }
            },
            Phone = "11999999999",
            Status = "Active",
            Role = "Customer"
        };
    }
}
