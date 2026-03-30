namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public class StoreUserAddress
{
    public string City { get; private set; } = string.Empty;
    public string Street { get; private set; } = string.Empty;
    public int Number { get; private set; }
    public string Zipcode { get; private set; } = string.Empty;
    public StoreUserGeolocation Geolocation { get; private set; } = null!;

    private StoreUserAddress()
    {
    }

    public StoreUserAddress(string city, string street, int number, string zipcode, StoreUserGeolocation geolocation)
    {
        Update(city, street, number, zipcode, geolocation);
    }

    public void Update(string city, string street, int number, string zipcode, StoreUserGeolocation geolocation)
    {
        City = city;
        Street = street;
        Number = number;
        Zipcode = zipcode;
        Geolocation = geolocation;
    }
}
