namespace Ambev.DeveloperEvaluation.WebApi.Features.Users;

public class StoreUserResponse
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public StoreUserNameResponse Name { get; set; } = new();
    public StoreUserAddressResponse Address { get; set; } = new();
    public string Phone { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
