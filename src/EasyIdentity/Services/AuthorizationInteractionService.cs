using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace EasyIdentity.Services;

public class AuthorizationInteractionService : IAuthorizationInteractionService
{
    private readonly ILogger<AuthorizationInteractionService> _logger;
    private readonly IDeviceCodeFlowManager _deviceCodeManager;
    private readonly IClientManager _clientManager;

    public AuthorizationInteractionService(ILogger<AuthorizationInteractionService> logger, IDeviceCodeFlowManager deviceCodeManager, IClientManager clientManager)
    {
        _logger = logger;
        _deviceCodeManager = deviceCodeManager;
        _clientManager = clientManager;
    }

    public async Task<bool> DeviceUserCodeAuthorizationAsync(string userCode, ClaimsPrincipal claimsPrincipal, bool grant, CancellationToken cancellationToken = default)
    {
        var deviceCode = await _deviceCodeManager.FindDeviceCodeAsync(userCode, cancellationToken);

        if (string.IsNullOrEmpty(deviceCode))
        {
            return false;
        }

        var clientId = await _deviceCodeManager.FindClientIdAsync(deviceCode, cancellationToken);

        if (string.IsNullOrEmpty(clientId))
            return false;

        var client = await _clientManager.FindByClientIdAsync(clientId, cancellationToken);

        if (client == null)
            return false;

        if (!await _deviceCodeManager.ValidateDeviceCodeAsync(deviceCode, client, cancellationToken))
            return false;

        if (grant)
            await _deviceCodeManager.GrantAsync(deviceCode, client, claimsPrincipal, cancellationToken);
        else
            await _deviceCodeManager.RejectAsync(deviceCode, client, claimsPrincipal, cancellationToken);

        return true;
    }

}
