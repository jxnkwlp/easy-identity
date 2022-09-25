using System.Threading;
using System.Threading.Tasks;

namespace EasyIdentity.Services;

public interface IRefreshTokenService
{
    Task<bool> IsValidAsync(string token, string[] scopes, CancellationToken cancellationToken = default);

    Task<string> GetSubjectAsync(string token, CancellationToken cancellationToken = default);

    Task<string[]> GetScopesAsync(string token, CancellationToken cancellationToken = default);
}
