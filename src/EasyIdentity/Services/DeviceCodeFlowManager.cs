using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;
using EasyIdentity.Stores;
using Microsoft.Extensions.Options;

namespace EasyIdentity.Services;

public class DeviceCodeFlowManager<TDeviceCode> : IDeviceCodeFlowManager where TDeviceCode : class
{
    private readonly EasyIdentityOptions _options;
    private readonly IDeviceCodeCodeCreationService _deviceCodeCodeCreationService;
    private readonly IDeviceCodeStore<TDeviceCode> _deviceCodeStore;

    public DeviceCodeFlowManager(IOptions<EasyIdentityOptions> options, IDeviceCodeCodeCreationService deviceCodeCodeCreationService, IDeviceCodeStore<TDeviceCode> deviceCodeStore)
    {
        _options = options.Value;
        _deviceCodeCodeCreationService = deviceCodeCodeCreationService;
        _deviceCodeStore = deviceCodeStore;
    }

    public async Task<bool> CheckGrantedAsync(string deviceCode, Client client, CancellationToken cancellationToken = default)
    {
        var item = await _deviceCodeStore.FindAsync(deviceCode, cancellationToken);
        if (item == null)
            return false;

        return (await _deviceCodeStore.GetGrantedAsync(item, cancellationToken)) ?? false;
    }

    public async Task<string> FindClientIdAsync(string deviceCode, CancellationToken cancellationToken = default)
    {
        var item = await _deviceCodeStore.FindAsync(deviceCode, cancellationToken);
        if (item == null)
            return string.Empty;

        return await _deviceCodeStore.GetClientIdAsync(item, cancellationToken);
    }

    public async Task<string> FindDeviceCodeAsync(string userCode, Client client = null, CancellationToken cancellationToken = default)
    {
        var item = await _deviceCodeStore.FindByUserCodeAsync(userCode, cancellationToken);
        if (item == null)
            return string.Empty;

        return await _deviceCodeStore.GetDeviceCodeAsync(item, cancellationToken);
    }

    public Task<string> FindDeviceCodeAsync(string userCode, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<string> FindSubjectAsync(string deviceCode, Client client, CancellationToken cancellationToken = default)
    {
        var item = await _deviceCodeStore.FindAsync(deviceCode, cancellationToken);
        if (item == null)
            return string.Empty;

        return await _deviceCodeStore.GetSubjectAsync(item, cancellationToken);
    }

    public async Task GrantAsync(string deviceCode, Client client, ClaimsPrincipal principal, CancellationToken cancellationToken = default)
    {
        var item = await _deviceCodeStore.FindAsync(deviceCode, cancellationToken);
        if (item == null)
            return;

        await _deviceCodeStore.SetGrantedAsync(item, true, cancellationToken);
    }

    public async Task RejectAsync(string deviceCode, Client client, ClaimsPrincipal principal, CancellationToken cancellationToken = default)
    {
        var item = await _deviceCodeStore.FindAsync(deviceCode, cancellationToken);
        if (item == null)
            return;

        await _deviceCodeStore.SetGrantedAsync(item, false, cancellationToken);
    }

    public async Task<DeviceCodeData> RequestAsync(RequestData requestData, RequestValidationResult validationResult, CancellationToken cancellationToken = default)
    {
        if (_options.DeviceCodeVerificationUri == null)
        {
            throw new Exception("The option 'DeviceCodeVerificationUri' is null");
        }

        var client = validationResult.Client;

        var deviceCode = await _deviceCodeCodeCreationService.CreateDeviceCodeAsync(client);
        var userCode = await _deviceCodeCodeCreationService.CreateUserCodeAsync(client);

        var data = new DeviceCodeData
        {
            DeviceCode = deviceCode,
            UserCode = userCode,
            ExpiresIn = (int)_options.DefaultDeviceCodeLifetime.TotalSeconds,
            Interval = _options.DefaultDevicePollingInterval,
            VerificationUri = _options.DeviceCodeVerificationUri?.ToString(),
        };

        await _deviceCodeStore.CreateAsync(data, client.ClientId, DateTime.UtcNow.Add(_options.DefaultDeviceCodeLifetime), cancellationToken);

        return data;
    }

    public async Task<bool> ValidateDeviceCodeAsync(string deviceCode, Client client, CancellationToken cancellationToken = default)
    {
        var item = await _deviceCodeStore.FindAsync(deviceCode, cancellationToken);
        if (item == null)
            return false;

        var expiration = await _deviceCodeStore.GetExpirationAsync(item, cancellationToken);

        if (expiration > DateTime.UtcNow)
            return true;

        return false;
    }
}
