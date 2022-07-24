using System;
using System.Linq;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public class AuthorizationCodeTokenRequestValidator : IGrantTypeTokenRequestValidator
{
    public string GrantType => GrantTypesConsts.AuthorizationCode;

    private readonly IClientManager _clientManager;
    private readonly IRedirectUrlValidator _redirectUrlValidator;
    private readonly IAuthorizationCodeFlowManager _authorizationCodeManager;

    public AuthorizationCodeTokenRequestValidator(IClientManager clientManager, IRedirectUrlValidator redirectUrlValidator, IAuthorizationCodeFlowManager authorizationCodeManager)
    {
        _clientManager = clientManager;
        _redirectUrlValidator = redirectUrlValidator;
        _authorizationCodeManager = authorizationCodeManager;
    }

    public async Task<RequestValidationResult> ValidateAsync(RequestData requestData)
    {
        var grantType = requestData.GrantType;
        var clientId = requestData.ClientId;
        var clientSecret = requestData.ClientSecret;
        var scope = requestData.Scope;
        var authorization = requestData.Authorization;
        var code = requestData.Code;
        var redirectUri = requestData.RedirectUri;

        if (string.IsNullOrEmpty(clientId))
        {
            return RequestValidationResult.Fail("invalid_request", "The client id is missing.");
        }

        var client = await _clientManager.FindByClientIdAsync(clientId);

        if (client.ClientSecretRequired && !string.IsNullOrEmpty(client.ClientSecret))
        {
            if (string.IsNullOrEmpty(clientSecret))
                return RequestValidationResult.Fail("invalid_request", "The client secret is required.");

            if (client.ClientSecret != clientSecret)
                return RequestValidationResult.Fail("invalid_client", "Invalid client secret.");
        }

        if (requestData.Scope?.Split(" ").Except(client.Scopes).Count() > 0)
            return RequestValidationResult.Fail("invalid_scope", "Invalid scope.");

        if (client.GrantTypes.Contains(requestData.GrantType) == false)
            return RequestValidationResult.Fail("unsupported_grant_type", "Invalid grant type.");

        if (string.IsNullOrWhiteSpace(code))
            return RequestValidationResult.Fail("invalid_request", "The code is missing.");

        if (string.IsNullOrWhiteSpace(redirectUri))
            return RequestValidationResult.Fail("invalid_request", "The redirect uri is missing.");

        if (!await _redirectUrlValidator.ValidateAsync(client, redirectUri))
            return RequestValidationResult.Fail("invalid_request", "The redirect uri not match.");

        // code validation
        var codeValidationResult = await _authorizationCodeManager.ValidationAsync(code, client, requestData);

        if (!codeValidationResult.Succeeded)
        {
            return RequestValidationResult.Fail("invalid_grant", codeValidationResult.Failure.Message);
        }

        return RequestValidationResult.Success(client, requestData, grantType);
    }
}
