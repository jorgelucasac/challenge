namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateStoreUser;

public class CreateStoreUserRequest
{
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public CreateStoreUserNameRequest Name { get; set; } = new();
    public CreateStoreUserAddressRequest Address { get; set; } = new();
    public string Phone { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
