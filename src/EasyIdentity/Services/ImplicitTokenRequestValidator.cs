using System.Threading.Tasks;
using EasyIdentity.Models;
using EasyIdentity.Stores;

namespace EasyIdentity.Services
{
    public class ImplicitTokenRequestValidator : IGrantTypeTokenRequestValidator
    {
        public string GrantType => GrantTypesConsts.Implicit;

        private readonly IClientStore _clientStore;

        public ImplicitTokenRequestValidator(IClientStore clientStore)
        {
            _clientStore = clientStore;
        }

        public async Task<RequestValidationResult> ValidateAsync(RequestData requestData)
        {
            var grantType = requestData.GrantType;
            var clientId = requestData.ClientId;
            var scope = requestData.Scope;
            var redirectUri = requestData.RedirectUri;

            if (string.IsNullOrEmpty(clientId))
                return RequestValidationResult.Fail("invalid_request");

            var client = await _clientStore.FindClientAsync(clientId);

            if (client == null)
                return RequestValidationResult.Fail("invalid_client", "Invalid client Id.");

            // if (string.IsNullOrWhiteSpace(redirectUri))
            // TODO

            //if (client.GrantTypes.Contains(grantType) == false)
            //    return RequestValidationResult.Fail("unsupported_grant_type", "Invalid grant type.");

            return RequestValidationResult.Success(client, requestData, grantType);
        }
    }
}
