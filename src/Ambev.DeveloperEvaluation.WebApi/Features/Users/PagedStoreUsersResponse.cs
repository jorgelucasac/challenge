namespace Ambev.DeveloperEvaluation.WebApi.Features.Users;

public class PagedStoreUsersResponse
{
    public List<StoreUserResponse> Data { get; set; } = [];
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}
