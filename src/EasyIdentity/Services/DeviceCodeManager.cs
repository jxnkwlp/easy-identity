using System;
using System.Security.Claims;
using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.Extensions.Options;

namespace EasyIdentity.Services;

public class DeviceCodeManager : IDeviceCodeManager
{
    private readonly EasyIdentityOptions _options;
    private readonly IDeviceCodeCodeCreationService _codeCreationService;
    private readonly IDeviceCodeStoreService _deviceCodeStoreService;

    public DeviceCodeManager(IOptions<EasyIdentityOptions> options, IDeviceCodeCodeCreationService codeCreationService, IDeviceCodeStoreService deviceCodeStoreService)
    {
        _options = options.Value;
        _codeCreationService = codeCreationService;
        _deviceCodeStoreService = deviceCodeStoreService;
    }

    public async Task<bool> CheckGrantedAsync(string deviceCode, Client client)
    {
        return await _deviceCodeStoreService.IsGrantedAsync(deviceCode);
    }

    public async Task<string> FindDeviceCodeAsync(string userCode, Client client = null)
    {
        return await _deviceCodeStoreService.FindDeviceCodeAsync(userCode);
    }

    public async Task<string> FindSubjectAsync(string deviceCode, Client client)
    {
        return await _deviceCodeStoreService.FindSubjectAsync(deviceCode);
    }

    public async Task GrantAsync(string deviceCode, Client client, ClaimsPrincipal principal)
    {
        await _deviceCodeStoreService.UpdateAsync(deviceCode, client.ClientId, principal, true);
    }

    public async Task RejectAsync(string deviceCode, Client client, ClaimsPrincipal principal)
    {
        await _deviceCodeStoreService.UpdateAsync(deviceCode, client.ClientId, principal, false);
    }

    public async Task<DeviceCodeData> RequestAsync(RequestData requestData, RequestValidationResult validationResult)
    {
        if (_options.DeviceCodeVerificationUri == null)
        {
            throw new Exception("The option 'DeviceCodeVerificationUri' is null");
        }

        var client = validationResult.Client;

        var deviceCode = await _codeCreationService.CreateDeviceCodeAsync(client);
        var userCode = await _codeCreationService.CreateUserCodeAsync(client);

        var data = new DeviceCodeData
        {
            DeviceCode = deviceCode,
            UserCode = userCode,
            ExpiresIn = (int)_options.DefaultDeviceCodeLifetime.TotalSeconds,
            Interval = _options.DefaultDevicePollingInterval,
            VerificationUri = _options.DeviceCodeVerificationUri?.ToString(),
        };

        await _deviceCodeStoreService.CreateAsync(data, client.ClientId, DateTime.UtcNow.Add(_options.DefaultDeviceCodeLifetime));

        return data;
    }

    public async Task<bool> ValidateDeviceCodeAsync(string deviceCode, Client client)
    {
        if (await _deviceCodeStoreService.IsExistsAsync(deviceCode))
        {
            if (!await _deviceCodeStoreService.IsExpirationAsync(deviceCode))
            {
                return true;
            }
        }

        return false;
    }

}
