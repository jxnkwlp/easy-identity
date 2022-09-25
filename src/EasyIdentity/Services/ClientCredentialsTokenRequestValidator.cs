using System.Linq;
using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Services;

public class ClientCredentialsTokenRequestValidator : IGrantTypeTokenRequestValidator
{
    public string GrantType => GrantTypeNameConsts.ClientCredentials;

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IClientManager _clientManager;

    public ClientCredentialsTokenRequestValidator(IHttpContextAccessor httpContextAccessor, IClientManager clientManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _clientManager = clientManager;
    }

    public async Task<RequestValidationResult> ValidateAsync(RequestData requestData)
    {
        var grantType = requestData.GrantType;
        var clientId = requestData.ClientId;
        var clientSecret = requestData.ClientSecret;
        var scope = requestData.Scope;
        var authorization = requestData.Authorization;

        if (string.IsNullOrEmpty(clientId))
        {
            return RequestValidationResult.Fail("invalid_request", "The request parameter 'client_id' is missing");
        }

        var client = await _clientManager.FindByClientIdAsync(clientId);

        if (client == null)
            return RequestValidationResult.Fail("invalid_client", "The client was invalid");

        if (string.IsNullOrEmpty(requestData.ClientSecret))
            return RequestValidationResult.Fail("invalid_client", "The request parameter 'client_secret' is missing");

        if (client.ClientSecret != requestData.ClientSecret)
            return RequestValidationResult.Fail("invalid_client", "The client authentication was invalid");

        if (requestData.Scope?.Split(" ").Except(client.Scopes).Count() > 0)
            return RequestValidationResult.Fail("invalid_scope", "Invalid scope.");

        if (client.GrantTypes.Contains(requestData.GrantType) == false)
            return RequestValidationResult.Fail("unsupported_grant_type", "Invalid grant type.");

        return RequestValidationResult.Success(client, requestData, grantType);
    }
}
