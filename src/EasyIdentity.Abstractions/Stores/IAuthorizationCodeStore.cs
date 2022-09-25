using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Stores;

/// <summary> 
///  An simple store interface for AuthorizationCode
/// </summary> 
public interface IAuthorizationCodeStore<TAuthorizationCode> where TAuthorizationCode : class
{
    Task CreateAsync(string code, string clientId, ClaimsPrincipal principal, DateTime expiration, RequestData requestData, CancellationToken cancellationToken = default);

    Task DeleteAsync(string code, CancellationToken cancellationToken = default);

    Task<TAuthorizationCode> FindAsync(string code, CancellationToken cancellationToken = default);

    Task<string> GetClientIdAsync(TAuthorizationCode authorizationCode, CancellationToken cancellationToken = default);

    Task<string> GetSubjectAsync(TAuthorizationCode authorizationCode, CancellationToken cancellationToken = default);

    Task<DateTime> GetExpirationAsync(TAuthorizationCode authorizationCode, CancellationToken cancellationToken = default);

    Task<string> GetRedirectUrlAsync(TAuthorizationCode authorizationCode, CancellationToken cancellationToken = default);

    Task<string> GetCodeChallengeAsync(TAuthorizationCode authorizationCode, CancellationToken cancellationToken = default);

    Task<string> GetCodeChallengeMethodAsync(TAuthorizationCode authorizationCode, CancellationToken cancellationToken = default);

    Task<string[]> GetScopesAsync(TAuthorizationCode authorizationCode, CancellationToken cancellationToken = default);

}
