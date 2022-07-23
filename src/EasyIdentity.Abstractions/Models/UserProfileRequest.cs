namespace EasyIdentity.Models;

public class UserProfileRequest
{
    public Client Client { get; }

    public string Subject { get; }

    public RequestData RequestData { get; }

    public UserProfileRequest(Client client, string subject, RequestData requestData)
    {
        Client = client;
        Subject = subject;
        RequestData = requestData;
    }
}
