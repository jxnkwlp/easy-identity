using System.Threading.Tasks;
using EasyIdentity.Models;
using EasyIdentity.Stores;

namespace EasyIdentity.Services
{
    public class AuthorizationCodeTokenRequestValidator : GrantTypeTokenRequestValidator, IGrantTypeTokenRequestValidator
    {
        public override string GrantType => GrantTypesConsts.AuthorizationCode;

        private readonly IClientStore _clientStore;
        private readonly IRedirectUrlValidator _redirectUrlValidator;

        public AuthorizationCodeTokenRequestValidator(IClientStore clientStore, IRedirectUrlValidator redirectUrlValidator)
        {
            _clientStore = clientStore;
            _redirectUrlValidator = redirectUrlValidator;
        }

        public override async Task<RequestValidationResult> ValidateAsync(RequestData requestData)
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

            var client = await _clientStore.FindClientAsync(clientId);

            var result = ValidateClient(client, requestData);

            if (!result.Succeeded)
                return result;

            // 
            if (string.IsNullOrWhiteSpace(code))
                return RequestValidationResult.Fail("invalid_request", "The code is missing.");

            if (string.IsNullOrWhiteSpace(redirectUri))
                return RequestValidationResult.Fail("invalid_request", "The redirect uri is missing.");

            if (!await _redirectUrlValidator.ValidateAsync(client, redirectUri))
                return RequestValidationResult.Fail("invalid_request", "The redirect uri not match.");

            return RequestValidationResult.Success(client, requestData, grantType);
        }
    }
}
