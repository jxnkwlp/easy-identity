using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Stores;

/// <summary>
///  Internal class
/// </summary>
public class DeviceCodeStore : IDeviceCodeStore<EasyIdentityDeviceCode>
{
    private static readonly List<EasyIdentityDeviceCode> _cache = new List<EasyIdentityDeviceCode>();

    public Task CreateAsync(DeviceCodeData data, string clientId, DateTime expiration, CancellationToken cancellationToken = default)
    {
        lock (_cache)
        {
            _cache.Add(new EasyIdentityDeviceCode
            {
                ClientId = clientId,
                DeviceCode = data.DeviceCode,
                UserCode = data.UserCode,
                Expiration = expiration,
            });
        }

        return Task.CompletedTask;
    }

    public Task RemoveAsync(string deviceCode, CancellationToken cancellationToken = default)
    {
        lock (_cache)
        {
            var item = _cache.FirstOrDefault(x => x.DeviceCode == deviceCode);
            if (item != null)
                _cache.Remove(item);
        }

        return Task.CompletedTask;
    }

    public Task<EasyIdentityDeviceCode> FindAsync(string deviceCode, CancellationToken cancellationToken = default)
    {
        EasyIdentityDeviceCode item = null;

        lock (_cache)
        {
            item = _cache.FirstOrDefault(x => x.DeviceCode == deviceCode);
        }

        return Task.FromResult(item);
    }

    public Task<EasyIdentityDeviceCode> FindByUserCodeAsync(string userCode, CancellationToken cancellationToken = default)
    {
        EasyIdentityDeviceCode item = null;

        lock (_cache)
        {
            item = _cache.FirstOrDefault(x => x.UserCode == userCode);
        }

        return Task.FromResult(item);
    }

    public Task<string> GetUserCodeAsync(EasyIdentityDeviceCode deviceCode, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(deviceCode.UserCode);
    }

    public Task<string> GetDeviceCodeAsync(EasyIdentityDeviceCode deviceCode, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(deviceCode.DeviceCode);
    }

    public Task<string> GetSubjectAsync(EasyIdentityDeviceCode deviceCode, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(deviceCode.Subject);
    }

    public Task<string> GetClientIdAsync(EasyIdentityDeviceCode deviceCode, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(deviceCode.ClientId);
    }

    public Task<bool?> GetGrantedAsync(EasyIdentityDeviceCode deviceCode, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(deviceCode.Granted);
    }

    public Task<DateTime> GetExpirationAsync(EasyIdentityDeviceCode deviceCode, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(deviceCode.Expiration);
    }

    public Task SetGrantedAsync(EasyIdentityDeviceCode deviceCode, bool granted, CancellationToken cancellationToken = default)
    {
        lock (_cache)
        {
            deviceCode.Granted = granted;
        }

        return Task.CompletedTask;
    }

    public Task SetSubjectAsync(EasyIdentityDeviceCode deviceCode, string subject, CancellationToken cancellationToken = default)
    {
        lock (_cache)
        {
            deviceCode.Subject = subject;
        }

        return Task.CompletedTask;
    }
}
