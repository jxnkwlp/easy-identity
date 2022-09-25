using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public interface ITokenManager : ITokenCreationService
{
    Task<TokenCreationResult> CreateAsync(Client client, string[] scopes, string subject, ClaimsPrincipal principal, RequestData requestData, CancellationToken cancellationToken = default);

    Task<bool> ValidateRefreshTokenAsync(string token, string[] scopes, CancellationToken cancellationToken = default);

    Task<string> GetRefreshTokenSubjectAsync(string token, CancellationToken cancellationToken = default);

    Task<string[]> GetRefreshTokenScopesAsync(string token, CancellationToken cancellationToken = default);

}
