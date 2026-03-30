namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public class StoreUserName
{
    public string Firstname { get; private set; } = string.Empty;
    public string Lastname { get; private set; } = string.Empty;

    private StoreUserName()
    {
    }

    public StoreUserName(string firstname, string lastname)
    {
        Update(firstname, lastname);
    }

    public void Update(string firstname, string lastname)
    {
        Firstname = firstname;
        Lastname = lastname;
    }
}
