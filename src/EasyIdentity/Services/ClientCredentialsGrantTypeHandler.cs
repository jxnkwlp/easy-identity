using System;
using System.Linq;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services
{
    public class ClientCredentialsGrantTypeHandler : IGrantTypeHandler
    {
        public string GrantType => GrantTypesConsts.ClientCredentials;

        private readonly ITokenCreationService _tokenGeneratorService;

        public ClientCredentialsGrantTypeHandler(ITokenCreationService tokenGeneratorService)
        {
            _tokenGeneratorService = tokenGeneratorService;
        }

        public async Task<GrantTypeHandleResult> HandleAsync(GrantTypeHandleRequest context)
        {
            var client = context.Client;

            // TODO 
            var tokenDescriptor = new TokenDescriptor(client.ClientId, client)
            {
                TokenType = "JWT",
                CreationTime = DateTime.UtcNow,
                Lifetime = 300,
            };
            //tokenDescriptor.Claims.Add( "typ", client.ClientName);
            //tokenDescriptor.Claims.Add("preferred_username", client.ClientName);
            //tokenDescriptor.Claims.Add("scope", string.Join(" ", client.Scopes));

            var accessToken = await _tokenGeneratorService.CreateTokenAsync(tokenDescriptor);

            string refreshToken = null;
            if (client.Scopes.Contains(StandardScopes.OfflineAccess))
            {
                refreshToken = await _tokenGeneratorService.CreateRefreshTokenAsync(tokenDescriptor);
            }

            return new GrantTypeHandleResult
            {
                ResponseData = new TokenResponseData
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    Scope = string.Join(" ", client.Scopes),
                    ExpiresIn = tokenDescriptor.Lifetime,
                    TokenType = "Bearer",
                }
            };
        }
    }
}
