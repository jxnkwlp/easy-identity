using System;
using System.Collections.Concurrent;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Extensions;
using EasyIdentity.Models;

namespace EasyIdentity.Stores;

/// <summary>
///  internal class
/// </summary>
public class AuthorizationCodeStore : IAuthorizationCodeStore<EasyIdentityAuthorizationCode>
{
    private static readonly ConcurrentDictionary<string, EasyIdentityAuthorizationCode> _cache = new ConcurrentDictionary<string, EasyIdentityAuthorizationCode>();

    public Task CreateAsync(string code, string clientId, ClaimsPrincipal principal, DateTime expiration, RequestData requestData, CancellationToken cancellationToken = default)
    {
        var subject = principal.GetSubject();

        _cache.TryAdd(code, new EasyIdentityAuthorizationCode
        {
            Subject = subject,
            Expiration = expiration,
            ClientId = clientId,
            Code = code,
            CodeChallenge = requestData.CodeChallenge,
            CodeChallengeMethod = requestData.CodeChallengeMethod,
            RedirectUrl = requestData.RedirectUri,
        });

        return Task.CompletedTask;
    }

    public Task<string> GetSubjectAsync(string code, CancellationToken cancellationToken = default)
    {
        if (_cache.TryGetValue(code, out var authorizationCode))
        {
            return Task.FromResult(authorizationCode.Subject);
        }
        else
            return Task.FromResult(string.Empty);
    }

    public Task<EasyIdentityAuthorizationCode> FindAsync(string code, CancellationToken cancellationToken = default)
    {
        if (_cache.TryGetValue(code, out var value))
        {
            return Task.FromResult(value);
        }

        return Task.FromResult<EasyIdentityAuthorizationCode>(default);
    }

    public Task DeleteAsync(string code, CancellationToken cancellationToken = default)
    {
        _cache.TryRemove(code, out var _);

        return Task.CompletedTask;
    }

    public Task<string> GetSubjectAsync(EasyIdentityAuthorizationCode authorizationCode, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(authorizationCode.Subject);
    }

    public Task<DateTime> GetExpirationAsync(EasyIdentityAuthorizationCode authorizationCode, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(authorizationCode.Expiration);
    }

    public Task<string> GetRedirectUrlAsync(EasyIdentityAuthorizationCode authorizationCode, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(authorizationCode.RedirectUrl);
    }

    public Task<string> GetCodeChallengeAsync(EasyIdentityAuthorizationCode authorizationCode, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(authorizationCode.CodeChallenge);
    }

    public Task<string> GetCodeChallengeMethodAsync(EasyIdentityAuthorizationCode authorizationCode, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(authorizationCode.CodeChallengeMethod);
    }

    public Task<string> GetClientIdAsync(EasyIdentityAuthorizationCode authorizationCode, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(authorizationCode.ClientId);
    }

    public Task<string[]> GetScopesAsync(EasyIdentityAuthorizationCode authorizationCode, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(authorizationCode.Scopes);
    }
}
