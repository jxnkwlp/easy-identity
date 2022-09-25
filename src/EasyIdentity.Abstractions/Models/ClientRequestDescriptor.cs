using System;

namespace EasyIdentity.Models;

[Obsolete]
public class ClientRequestDescriptor
{
    public ClientRequestDescriptor(string clientId, string grantType, string[] scopes, Client origin)
    {
        ClientId = clientId;
        GrantType = grantType;
        Scopes = scopes;
        Origin = origin;
    }

    public string ClientId { get; }

    public string GrantType { get; }

    public string[] Scopes { get; }

    public Client Origin { get; }
}
