using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;
using EasyIdentity.Stores;

namespace EasyIdentity.Services;


public class RefreshTokenService : RefreshTokenService<EasyIdentityToken>
{
    public RefreshTokenService(ITokenStore<EasyIdentityToken> tokenStore) : base(tokenStore)
    {
    }
}

public class RefreshTokenService<TToken> : IRefreshTokenService where TToken : class
{
    private readonly ITokenStore<TToken> _tokenStore;

    public RefreshTokenService(ITokenStore<TToken> tokenStore)
    {
        _tokenStore = tokenStore;
    }

    public async Task<string[]> GetScopesAsync(string token, CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(token, out var id))
            return default;

        var tokenObj = await _tokenStore.FindByIdAsync(id, cancellationToken);

        if (tokenObj == null)
            return default;

        return (await _tokenStore.GetScopeAsync(tokenObj, cancellationToken))?.Split(' ');
    }

    public async Task<string> GetSubjectAsync(string token, CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(token, out var id))
            return default;

        var tokenObj = await _tokenStore.FindByIdAsync(id, cancellationToken);

        if (tokenObj == null)
            return default;

        return await _tokenStore.GetSubjectAsync(tokenObj, cancellationToken);
    }

    public async Task<bool> IsValidAsync(string token, string[] scopes, CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(token, out var id))
            return default;

        var tokenObj = await _tokenStore.FindByIdAsync(id, cancellationToken);

        if (tokenObj == null)
            return false;

        var tokenScopes = (await _tokenStore.GetScopeAsync(tokenObj, cancellationToken))?.Split(' ');
        if (tokenScopes.Except(scopes).Count() != 0)
            return false;

        var lifetime = await _tokenStore.GetLifetimeAsync(tokenObj, cancellationToken);
        var creationTime = await _tokenStore.GetCreationTimeAsync(tokenObj, cancellationToken);

        return DateTime.UtcNow <= creationTime.AddSeconds(lifetime);
    }
}
