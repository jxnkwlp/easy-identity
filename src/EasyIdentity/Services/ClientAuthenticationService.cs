using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.Extensions.Logging;

namespace EasyIdentity.Services;

public class ClientAuthenticationService : IClientAuthenticationService
{
    private ILogger<ClientAuthenticationService> _logger;
    private IClientManager _clientManager;

    public async Task<ClientAuthenticationResult> ValidateAsync(string clientId, string grantType, RequestData data, CancellationToken cancellationToken = default)
    {
        var clientSecret = data.ClientSecret;
        var scopes = data.Scopes;

        var client = await _clientManager.FindByClientIdAsync(clientId, cancellationToken);

        if (client == null)
            return ClientAuthenticationResult.Fail(IdentityError.Create("invalid_client"));

        if (!string.IsNullOrEmpty(client.ClientSecret) && client.ClientSecret != clientSecret)
            return ClientAuthenticationResult.Fail(IdentityError.Create("invalid_client"));

        if (client.Scopes.Except(scopes).Count() > 0)
            return ClientAuthenticationResult.Fail(IdentityError.Create("invalid_scope"));

        return ClientAuthenticationResult.Success(client, grantType);
    }
}
