namespace EasyIdentity.Models;

/// <summary>
///  The command request validation result class
/// </summary>
public class RequestValidationResult
{
    public Client Client { get; protected set; }

    public string GrantType { get; protected set; }

    public RequestData Data { get; protected set; }

    public bool Succeeded => string.IsNullOrEmpty(Error);

    public string Error { get; protected set; }

    public string ErrorDescription { get; protected set; }

    protected RequestValidationResult()
    {
    }

    public static RequestValidationResult Fail(string error, string errorDescription = null)
    {
        return new RequestValidationResult
        {
            Error = error,
            ErrorDescription = errorDescription
        };
    }

    public static RequestValidationResult Success(Client client, RequestData requestData, string grantType = null)
    {
        return new RequestValidationResult
        {
            GrantType = grantType,
            Client = client,
            // ClientRequestDescriptor = new ClientRequestDescriptor(requestData.ClientId, requestData.GrantType, requestData.Scopes, client),
            Data = requestData
        };
    }
}
