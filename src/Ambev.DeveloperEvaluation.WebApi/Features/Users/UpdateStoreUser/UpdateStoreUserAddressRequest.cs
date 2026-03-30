namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateStoreUser;

public class UpdateStoreUserAddressRequest
{
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public int Number { get; set; }
    public string Zipcode { get; set; } = string.Empty;
    public UpdateStoreUserGeolocationRequest Geolocation { get; set; } = new();
}
