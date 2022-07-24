using System;
using System.Linq;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public class AuthorizationRequestValidator : IAuthorizationRequestValidator
{
    private readonly IClientManager _clientManager;

    public AuthorizationRequestValidator(IClientManager clientManager)
    {
        _clientManager = clientManager;
    }

    public async Task<RequestValidationResult> ValidateAsync(RequestData requestData)
    {
        var grantType = requestData.GrantType;
        var clientId = requestData.ClientId;
        var scope = requestData.Scope;
        var code = requestData.Code;
        var redirectUri = requestData.RedirectUri;
        var responseType = requestData.ResponseType;

        if (string.IsNullOrEmpty(scope))
        {
            return RequestValidationResult.Fail("invalid_request");
        }

        var client = await _clientManager.FindByClientIdAsync(clientId);

        if (client == null)
            return RequestValidationResult.Fail("invali_client", "Invalid client.");

        //if (scope.Split(" ").Except(client.Scopes).Count() > 0)
        //    return RequestValidationResult.Fail("invalid_scope", "Invalid scope.");

        //if (client.RedirectUrls?.Contains(redirectUri) == false)
        //    return RequestValidationResult.Fail("invalid_scope", "Invalid scope.");

        if (responseType != "code" && responseType != "token")
        {
            // TODO 
        }

        if (responseType == "code" && !client.GrantTypes.Contains(GrantTypesConsts.AuthorizationCode))
            return RequestValidationResult.Fail("invalid_request", "");

        if (responseType == "token" && !client.GrantTypes.Contains(GrantTypesConsts.Implicit))
            return RequestValidationResult.Fail("invalid_request", "");

        return RequestValidationResult.Success(client, requestData);
    }
}
