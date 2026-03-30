namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateStoreUser;

public class CreateStoreUserAddressRequest
{
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public int Number { get; set; }
    public string Zipcode { get; set; } = string.Empty;
    public CreateStoreUserGeolocationRequest Geolocation { get; set; } = new();
}
