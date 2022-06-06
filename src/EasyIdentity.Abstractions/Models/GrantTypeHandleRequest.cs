using System.Collections.Generic;

namespace EasyIdentity.Models
{
    public class GrantTypeHandleRequest
    {
        public GrantTypeHandleRequest(Client client, Scope[] scopes, Dictionary<string, string> rawData)
        {
            Client = client;
            Scopes = scopes;
            RawData = rawData;
        }

        public Client Client { get; protected set; }
        public Scope[] Scopes { get; protected set; }
        public Dictionary<string, string> RawData { get; protected set; }

    }
}
