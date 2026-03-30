namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public class StoreUserGeolocation
{
    public string Lat { get; private set; } = string.Empty;
    public string Long { get; private set; } = string.Empty;

    private StoreUserGeolocation()
    {
    }

    public StoreUserGeolocation(string lat, string @long)
    {
        Update(lat, @long);
    }

    public void Update(string lat, string @long)
    {
        Lat = lat;
        Long = @long;
    }
}
