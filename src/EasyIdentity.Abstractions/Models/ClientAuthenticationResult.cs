namespace EasyIdentity.Models;

public class ClientAuthenticationResult
{
    public Client Client { get; protected set; }

    public string GrantType { get; protected set; }

    public bool Succeeded => Error == null;

    public IdentityError Error { get; protected set; }

    protected ClientAuthenticationResult()
    {
    }

    public static ClientAuthenticationResult Fail(IdentityError error)
    {
        return new ClientAuthenticationResult
        {
            Error = error,
        };
    }

    public static ClientAuthenticationResult Success(Client client, string grantType)
    {
        return new ClientAuthenticationResult { GrantType = grantType, Client = client };
    }
}
