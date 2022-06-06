using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyIdentity.Models;
using EasyIdentity.Stores;

namespace EasyIdentity.Services
{
    public class AuthorizationCodeTokenRequestValidator : IGrantTypeITokenRequestValidator
    {
        public string GrantType => GrantTypesConsts.AuthorizationCode;

        private readonly IClientStore _clientStore;

        public AuthorizationCodeTokenRequestValidator(IClientStore clientStore)
        {
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
            var code = requestData["code"]; 
            var redirectUri = requestData["redirect_uri"];

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

            if (string.IsNullOrEmpty(clientId))
                return RequestValidationResult.Fail("invalid_request");

            var client = await _clientStore.FindClientAsync(clientId);

            if (client == null)
                return RequestValidationResult.Fail("invalid_client", "Invalid client Id.");

            if (client.ClientSecretRequired && string.IsNullOrEmpty(clientSecret))
                return RequestValidationResult.Fail("invalid_request", "The client secret is required.");

            if (client.ClientSecretRequired && client.ClientSecret != clientSecret)
                return RequestValidationResult.Fail("invalid_client", "Invalid client secret.");

            if (string.IsNullOrWhiteSpace(code))
                return RequestValidationResult.Fail("invalid_request");

            if (string.IsNullOrWhiteSpace(redirectUri))
                return RequestValidationResult.Fail("invalid_request");

            if (client.GrantTypes.Contains(grantType) == false)
                return RequestValidationResult.Fail("unsupported_grant_type", "Invalid grant type.");

            return RequestValidationResult.Success(client, requestData, grantType);
        }
    }
}
