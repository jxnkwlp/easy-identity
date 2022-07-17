namespace EasyIdentity.Models
{
    public class GrantTypeHandleRequest
    {
        public string Subject { get; }
        public Client Client { get; }
        public Scope[] Scopes { get; }
        public RequestData Data { get; }

        public GrantTypeHandleRequest(string subject, Client client, Scope[] scopes, RequestData requestData)
        {
            Subject = subject;
            Client = client;
            Scopes = scopes;
            Data = requestData;
        }
    }
}
