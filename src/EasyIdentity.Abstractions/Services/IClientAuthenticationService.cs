using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public interface IClientAuthenticationService
{
    Task<ClientAuthenticationResult> ValidateAsync(string clientId, string grantType, RequestData data, CancellationToken cancellationToken = default);
}
