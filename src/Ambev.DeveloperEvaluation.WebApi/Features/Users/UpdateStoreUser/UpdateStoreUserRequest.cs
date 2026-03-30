namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateStoreUser;

public class UpdateStoreUserRequest
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UpdateStoreUserNameRequest Name { get; set; } = new();
    public UpdateStoreUserAddressRequest Address { get; set; } = new();
    public string Phone { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
