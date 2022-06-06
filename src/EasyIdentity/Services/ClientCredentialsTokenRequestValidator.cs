using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyIdentity.Models;
using EasyIdentity.Stores;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Services
{
    public class ClientCredentialsTokenRequestValidator : IGrantTypeITokenRequestValidator
    {
        public string GrantType => GrantTypesConsts.ClientCredentials;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IClientStore _clientStore;

        public ClientCredentialsTokenRequestValidator(IHttpContextAccessor httpContextAccessor, IClientStore clientStore)
        {
            _httpContextAccessor = httpContextAccessor;
            _clientStore = clientStore;
        }

        private static bool TryDecodeBase64String(string value, out string source)
        {
            source = string.Empty;
            Span<byte> buffer = new Span<byte>(new byte[value.Length]);
            if (Convert.TryFromBase64String(value, buffer, out int length))
            {
                source = Encoding.UTF8.GetString(buffer.ToArray());
                return true;
            }

            return false;
        }

        public async Task<RequestValidationResult> ValidateAsync(RequestData requestData)
        {
            var grantType = requestData["grant_type"];
            var clientId = requestData["client_id"];
            var clientSecret = requestData["client_secret"];
            var scope = requestData["scope"];
            var authorization = requestData["authorization"];

            if (!string.IsNullOrEmpty(authorization))
            {
                if (TryDecodeBase64String(authorization, out var value))
                {
                    if (value.IndexOf(":") > 0)
                    {
                        clientId = value.Substring(0, value.IndexOf(":"));
                        clientSecret = value.Substring(value.IndexOf(":") + 1);
                    }
                    else
                    {
                        clientId = value;
                    }
                }
            }

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

            return RequestValidationResult.Success(client, requestData, grantType);
        }
    }
}
