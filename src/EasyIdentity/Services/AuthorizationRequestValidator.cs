using System;
using System.Linq;
using System.Threading.Tasks;
using EasyIdentity.Models;
using EasyIdentity.Stores;

namespace EasyIdentity.Services
{
    public class AuthorizationRequestValidator : IAuthorizationRequestValidator
    {
        private readonly IClientStore _clientStore;

        public AuthorizationRequestValidator(IClientStore clientStore)
        {
            _clientStore = clientStore;
        }

        public async Task<RequestValidationResult> ValidateAsync(RequestData requestData)
        {
            var clientId = requestData["client_id"];
            var scope = requestData["scope"];
            var responseType = requestData["response_type"];
            var redirectUri = requestData["redirect_uri"];
            var state = requestData["state"];

            if (string.IsNullOrEmpty(scope))
            {
                return RequestValidationResult.Fail("invalid_request");
            }

            var client = await _clientStore.FindClientAsync(clientId);

            if (client == null)
                return RequestValidationResult.Fail("invalid_scope", "Invalid scope.");

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
}
