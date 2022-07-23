using System;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services
{
    [Obsolete]
    public abstract class GrantTypeTokenRequestValidator : IGrantTypeTokenRequestValidator
    {
        public abstract string GrantType { get; }

        public abstract Task<RequestValidationResult> ValidateAsync(RequestData data);

        protected RequestValidationResult ValidateClient(Client client, RequestData requestData)
        {
            //if (client.ClientSecretRequired && string.IsNullOrEmpty(requestData.ClientSecret))
            //    return RequestValidationResult.Fail("invalid_request", "The client secret missing.");

            //if (client.ClientSecretRequired && client.ClientSecret != requestData.ClientSecret)
            //    return RequestValidationResult.Fail("invalid_client", "Invalid client secret.");

            //if (requestData.Scope?.Split(" ").Except(client.Scopes).Count() > 0)
            //    return RequestValidationResult.Fail("invalid_scope", "Invalid scope.");

            //if (client.GrantTypes.Contains(requestData.GrantType) == false)
            //    return RequestValidationResult.Fail("unsupported_grant_type", "Invalid grant type.");

            return RequestValidationResult.Success(client, requestData, null);
        }

    }
}
