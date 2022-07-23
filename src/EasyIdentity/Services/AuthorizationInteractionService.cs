using System.Security.Claims;
using System.Threading.Tasks;
using EasyIdentity.Stores;
using Microsoft.Extensions.Logging;

namespace EasyIdentity.Services;

public class AuthorizationInteractionService : IAuthorizationInteractionService
{
    private readonly ILogger<AuthorizationInteractionService> _logger;
    private readonly IDeviceCodeManager _deviceCodeManager;
    private readonly IDeviceCodeStoreService _deviceCodeStoreService;
    private readonly IClientStore _clientStore;

    public AuthorizationInteractionService(ILogger<AuthorizationInteractionService> logger, IDeviceCodeManager deviceCodeManager, IDeviceCodeStoreService deviceCodeStoreService, IClientStore clientStore)
    {
        _logger = logger;
        _deviceCodeManager = deviceCodeManager;
        _deviceCodeStoreService = deviceCodeStoreService;
        _clientStore = clientStore;
    }

    public async Task<bool> DeviceUserCodeAuthorizationAsync(string userCode, ClaimsPrincipal claimsPrincipal, bool grant)
    {
        var deviceCode = await _deviceCodeManager.FindDeviceCodeAsync(userCode, null);

        if (string.IsNullOrEmpty(deviceCode))
        {
            return false;
        }

        var clientId = await _deviceCodeStoreService.FindClientIdAsync(deviceCode);

        if (string.IsNullOrEmpty(clientId))
            return false;

        var client = await _clientStore.FindClientAsync(clientId);

        if (client == null)
            return false;

        if (!await _deviceCodeManager.ValidateDeviceCodeAsync(deviceCode, client))
            return false;

        if (grant)
            await _deviceCodeManager.GrantAsync(deviceCode, client, claimsPrincipal);
        else
            await _deviceCodeManager.RejectAsync(deviceCode, client, claimsPrincipal);

        return true;
    }

}
