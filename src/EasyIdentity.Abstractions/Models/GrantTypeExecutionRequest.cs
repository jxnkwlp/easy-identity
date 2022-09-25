using System.Security.Claims;

namespace EasyIdentity.Models;

public class GrantTypeExecutionRequest
{
    public GrantTypeExecutionRequest(string subject, Client client, ClaimsPrincipal claimsPrincipal, RequestData data)
    {
        Subject = subject;
        Client = client;
        ClaimsPrincipal = claimsPrincipal;
        Data = data;
    }

    public string Subject { get; }
    public Client Client { get; }
    public ClaimsPrincipal ClaimsPrincipal { get; }
    public RequestData Data { get; }
     
}
