namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.ListStoreUsers;

public class ListStoreUsersRequest
{
    public int _page { get; set; } = 1;
    public int _size { get; set; } = 10;
    public string _order { get; set; } = "id asc";
}
