namespace Ambev.DeveloperEvaluation.WebApi.Features.Users;

public class StoreUserAddressResponse
{
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public int Number { get; set; }
    public string Zipcode { get; set; } = string.Empty;
    public StoreUserGeolocationResponse Geolocation { get; set; } = new();
}
