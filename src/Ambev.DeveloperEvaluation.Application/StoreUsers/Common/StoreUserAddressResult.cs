namespace Ambev.DeveloperEvaluation.Application.StoreUsers.Common;

public class StoreUserAddressResult
{
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public int Number { get; set; }
    public string Zipcode { get; set; } = string.Empty;
    public StoreUserGeolocationResult Geolocation { get; set; } = new();
}
