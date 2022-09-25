using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Stores;

/// <summary> 
///  An simple store interface for Token
/// </summary> 
public interface ITokenStore<TToken> where TToken : class
{
    Task<TToken> CreateAsync(TokenDescriptor token, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task DeleteBySubjectAsync(string subject, CancellationToken cancellationToken = default);
    Task DeleteByClientIdAsync(string clientId, CancellationToken cancellationToken = default);

    Task<TToken> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<string> GetSubjectAsync(TToken token, CancellationToken cancellationToken = default);
    Task<string> GetClientIdAsync(TToken token, CancellationToken cancellationToken = default);
    Task<string> GetTokenTypeAsync(TToken token, CancellationToken cancellationToken = default);
    Task<string> GetTokenNameAsync(TToken token, CancellationToken cancellationToken = default);
    Task<string> GetIssuerAsync(TToken token, CancellationToken cancellationToken = default);
    Task<string> GetAudiencesAsync(TToken token, CancellationToken cancellationToken = default);
    Task<string> GetScopeAsync(TToken token, CancellationToken cancellationToken = default);
    Task<int> GetLifetimeAsync(TToken token, CancellationToken cancellationToken = default);
    Task<DateTime> GetCreationTimeAsync(TToken token, CancellationToken cancellationToken = default);
    Task<Dictionary<string, string>> GetClaimsAsync(TToken token, CancellationToken cancellationToken = default);

}
