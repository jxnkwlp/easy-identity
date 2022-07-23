using System;
using System.Linq;
using System.Threading.Tasks;
using EasyIdentity.Models;
using EasyIdentity.Stores;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Services;

public class PasswordTokenRequestValidator : IGrantTypeTokenRequestValidator
{
    public string GrantType => GrantTypesConsts.Password;

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IClientStore _clientStore;

    public PasswordTokenRequestValidator(IHttpContextAccessor httpContextAccessor, IClientStore clientStore)
    {
        _httpContextAccessor = httpContextAccessor;
        _clientStore = clientStore;
    }

    public async Task<RequestValidationResult> ValidateAsync(RequestData requestData)
    {
        var grantType = requestData["grant_type"];
        var clientId = requestData["client_id"];
        var clientSecret = requestData["client_secret"];
        var scope = requestData["scope"];
        var authorization = requestData["authorization"];
        var username = requestData["username"];
        var password = requestData["password"];

        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(scope))
        {
            return RequestValidationResult.Fail("invalid_request");
        }

        var client = await _clientStore.FindClientAsync(clientId);

        if (client == null)
            return RequestValidationResult.Fail("invalid_client", "Invalid client Id.");

        if (client.ClientSecretRequired && string.IsNullOrEmpty(clientSecret))
            return RequestValidationResult.Fail("invalid_request", "The client secret is required.");

        if (client.ClientSecretRequired && client.ClientSecret != clientSecret)
            return RequestValidationResult.Fail("invalid_client", "Invalid client secret.");

        if (scope.Split(" ").Except(client.Scopes).Count() > 0)
            return RequestValidationResult.Fail("invalid_scope", "Invalid scope.");

        if (client.GrantTypes.Contains(grantType) == false)
            return RequestValidationResult.Fail("unsupported_grant_type", "Invalid grant type.");

        if (string.IsNullOrEmpty(username))
            return RequestValidationResult.Fail("invalid_grant", "Invalid username.");

        if (string.IsNullOrEmpty(password))
            return RequestValidationResult.Fail("invalid_grant", "Invalid password.");

        return RequestValidationResult.Success(client, requestData, grantType);
    }
}
