using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Services;

public class ClientCredentialsTokenRequestValidator : GrantTypeTokenRequestValidator, IGrantTypeTokenRequestValidator
{
    public override string GrantType => GrantTypesConsts.ClientCredentials;

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IClientManager _clientManager;

    public ClientCredentialsTokenRequestValidator(IHttpContextAccessor httpContextAccessor, IClientManager clientManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _clientManager = clientManager;
    }

    public override async Task<RequestValidationResult> ValidateAsync(RequestData requestData)
    {
        var grantType = requestData.GrantType;
        var clientId = requestData.ClientId;
        var clientSecret = requestData.ClientSecret;
        var scope = requestData.Scope;
        var authorization = requestData.Authorization;

        if (string.IsNullOrEmpty(clientId))
        {
            return RequestValidationResult.Fail("invalid_request", "Client id missing.");
        }

        var client = await _clientManager.FindByClientIdAsync(clientId);

        var result = ValidateClient(client, requestData);

        if (!result.Succeeded)
            return result;

        return RequestValidationResult.Success(client, requestData, grantType);
    }
}
