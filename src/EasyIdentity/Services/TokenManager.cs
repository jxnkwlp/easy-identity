using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;
using EasyIdentity.Stores;
using Microsoft.Extensions.Options;

namespace EasyIdentity.Services;

public class TokenManager<TToken> : ITokenManager where TToken : class
{
    private readonly EasyIdentityOptions _options;
    private readonly ITokenCreationService _tokenCreationService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly ITokenStore<TToken> _tokenStore;

    public TokenManager(IOptions<EasyIdentityOptions> options, ITokenCreationService tokenCreationService, IRefreshTokenService refreshTokenService, ITokenStore<TToken> tokenStore)
    {
        _options = options.Value;
        _tokenCreationService = tokenCreationService;
        _refreshTokenService = refreshTokenService;
        _tokenStore = tokenStore;
    }

    public async Task<TokenCreationResult> CreateAsync(Client client, string[] scopes, string subject, ClaimsPrincipal principal, RequestData requestData, CancellationToken cancellationToken = default)
    {
        var tokenDescriptor = new TokenDescriptor(subject, client, principal)
        {
            Issuer = _options.Issuer,
            Lifetime = _options.DefaultAccessTokenLifetime,
            TokenName = "Access",
            Audiences = client.ClientId,
            CreationTime = DateTime.UtcNow,
        };

        // create
        var accessToken = await CreateAccessTokenAsync(tokenDescriptor);
        // save 
        await _tokenStore.CreateAsync(tokenDescriptor);

        string identityToken = null;
        TokenDescriptor identityTokenDescriptor = null;
        if (scopes.Contains(StandardScopes.OpenId))
        {
            identityTokenDescriptor = tokenDescriptor;
            identityTokenDescriptor.Guid = Guid.NewGuid();
            identityTokenDescriptor.Lifetime = _options.DefaultIdentityTokenLifetime;
            identityTokenDescriptor.TokenName = "Identity";
            // create
            identityToken = await CreateIdentityTokenAsync(identityTokenDescriptor, accessToken);
            // save 
            await _tokenStore.CreateAsync(identityTokenDescriptor);
        }

        string refreshToken = null;
        TokenDescriptor refreshTokenDescriptor = null;
        if (scopes.Contains(StandardScopes.OfflineAccess))
        {
            refreshTokenDescriptor = tokenDescriptor;
            refreshTokenDescriptor.Guid = Guid.NewGuid();
            refreshTokenDescriptor.Lifetime = _options.DefaultRefreshTokenLifetime;
            refreshTokenDescriptor.TokenName = "Refresh";
            // create
            refreshToken = await CreateRefreshTokenAsync(refreshTokenDescriptor, accessToken);
            // save 
            await _tokenStore.CreateAsync(refreshTokenDescriptor);
        }

        return new TokenCreationResult(tokenDescriptor, accessToken, identityTokenDescriptor, identityToken, refreshTokenDescriptor, refreshToken);
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

    public async Task<bool> ValidateRefreshTokenAsync(string token, string[] scopes, CancellationToken cancellationToken = default)
    {
        return await _refreshTokenService.IsValidAsync(token, scopes, cancellationToken);
    }

    public async Task<string> GetRefreshTokenSubjectAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _refreshTokenService.GetSubjectAsync(token, cancellationToken);
    }

    public async Task<string[]> GetRefreshTokenScopesAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _refreshTokenService.GetScopesAsync(token, cancellationToken);
    }
}
