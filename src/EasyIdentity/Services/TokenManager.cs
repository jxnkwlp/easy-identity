using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services
{
    public class TokenManager : ITokenManager
    {
        private readonly ITokenCreationService _tokenCreationService;
        private readonly EasyIdentityOptions _options;

        public TokenManager(ITokenCreationService tokenCreationService, EasyIdentityOptions options)
        {
            _tokenCreationService = tokenCreationService;
            _options = options;
        }

        public async Task<TokenCreationResult> CreateAsync(string subject, Client client, ClaimsPrincipal principal)
        {
            var token = new TokenDescriptor(subject, client, principal)
            {
                Lifetime = _options.DefaultAccessTokenLifetime,
                TokenName = "AccessToken",
                Issuer = _options.Issuer,
                Audiences = client.ClientId,
                CreationTime = DateTime.UtcNow,
            };

            var accessToken = await CreateAccessTokenAsync(token);

            string identityToken = null;
            if (client.Scopes.Contains(StandardScopes.OpenId))
            {
                token.Lifetime = _options.DefaultIdentityTokenLifetime;
                token.TokenName = "IdentityToken";
                identityToken = await CreateIdentityTokenAsync(token, accessToken);
            }

            string refreshToken = null;
            if (client.Scopes.Contains(StandardScopes.OfflineAccess))
            {
                token.Lifetime = _options.DefaultRefreshTokenLifetime;
                token.TokenName = "RefreshToken";
                refreshToken = await CreateRefreshTokenAsync(token, accessToken);
            }

            return new TokenCreationResult(token, accessToken, identityToken, refreshToken);
        }

        public async Task<string> CreateAccessTokenAsync(TokenDescriptor tokenDescriptor)
        {
            return await _tokenCreationService.CreateAccessTokenAsync(tokenDescriptor);
        }

        public async Task<string> CreateIdentityTokenAsync(TokenDescriptor tokenDescriptor, string accessToken)
        {
            return await _tokenCreationService.CreateIdentityTokenAsync(tokenDescriptor, accessToken);
        }

        public async Task<string> CreateRefreshTokenAsync(TokenDescriptor tokenDescriptor, string accessToken)
        {
            return await _tokenCreationService.CreateRefreshTokenAsync(tokenDescriptor, accessToken);
        }

    }
}
