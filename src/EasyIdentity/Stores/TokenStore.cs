using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Stores;

/// <summary>
///  Internal class
/// </summary>
public class TokenStore : ITokenStore<EasyIdentityToken>
{
    private static readonly List<EasyIdentityToken> _cache = new List<EasyIdentityToken>();

    public Task<EasyIdentityToken> CreateAsync(TokenDescriptor token, CancellationToken cancellationToken = default)
    {
        var tokenCache = new EasyIdentityToken()
        {
            Id = token.Guid,
            Audiences = token.Audiences,
            Claims = token.Principal.Claims.ToDictionary(x => x.Type, x => x.Value),
            ClientId = token.Client.ClientId,
            CreationTime = token.CreationTime,
            Issuer = token.Issuer,
            Lifetime = (int)token.Lifetime.TotalSeconds,
            Scope = token.Scope,
            Subject = token.Subject,
            TokenName = token.TokenName,
            TokenType = token.TokenType,
        };

        lock (_cache)
        {
            _cache.Add(tokenCache);
        }

        return Task.FromResult(tokenCache);
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        lock (_cache)
        {
            var item = _cache.FirstOrDefault(x => x.Id == id);
            if (item != null)
                _cache.Remove(item);
        }

        return Task.CompletedTask;
    }

    public Task DeleteByClientIdAsync(string clientId, CancellationToken cancellationToken = default)
    {
        lock (_cache)
        {
            _cache.RemoveAll(x => x.ClientId == clientId);
        }

        return Task.CompletedTask;
    }

    public Task DeleteBySubjectAsync(string subject, CancellationToken cancellationToken = default)
    {
        lock (_cache)
        {
            _cache.RemoveAll(x => x.Subject == subject);
        }

        return Task.CompletedTask;
    }

    public Task<EasyIdentityToken> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        lock (_cache)
        {
            var item = _cache.FirstOrDefault(x => x.Id == id);

            return Task.FromResult(item);
        }
    }

    public Task<string> GetAudiencesAsync(EasyIdentityToken token, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(token.Audiences);
    }

    public Task<Dictionary<string, string>> GetClaimsAsync(EasyIdentityToken token, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(token.Claims);
    }

    public Task<string> GetClientIdAsync(EasyIdentityToken token, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(token.ClientId);
    }

    public Task<DateTime> GetCreationTimeAsync(EasyIdentityToken token, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(token.CreationTime);
    }

    public Task<string> GetIssuerAsync(EasyIdentityToken token, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(token.Audiences);
    }

    public Task<int> GetLifetimeAsync(EasyIdentityToken token, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(token.Lifetime);
    }

    public Task<string> GetScopeAsync(EasyIdentityToken token, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(token.Scope);
    }

    public Task<string> GetSubjectAsync(EasyIdentityToken token, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(token.Subject);
    }

    public Task<string> GetTokenNameAsync(EasyIdentityToken token, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(token.TokenName);
    }

    public Task<string> GetTokenTypeAsync(EasyIdentityToken token, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(token.TokenType);
    }
}
