using System.Threading.Tasks;
using EasyIdentity.Models;
using EasyIdentity.Stores;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Services
{
    public class ClientCredentialsTokenRequestValidator : GrantTypeTokenRequestValidator, IGrantTypeTokenRequestValidator
    {
        public override string GrantType => GrantTypesConsts.ClientCredentials;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IClientStore _clientStore;

        public ClientCredentialsTokenRequestValidator(IHttpContextAccessor httpContextAccessor, IClientStore clientStore)
        {
            _httpContextAccessor = httpContextAccessor;
            _clientStore = clientStore;
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

            var client = await _clientStore.FindClientAsync(clientId);

            var result = ValidateClient(client, requestData);

            if (!result.Succeeded)
                return result;

            return RequestValidationResult.Success(client, requestData, grantType);
        }
    }
}
