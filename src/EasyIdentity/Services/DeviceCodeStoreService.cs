using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EasyIdentity.Extensions;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public class DeviceCodeStoreService : IDeviceCodeStoreService
{
    private static readonly ConcurrentDictionary<string, DeviceCodeModel> _cache = new ConcurrentDictionary<string, DeviceCodeModel>();

    public Task<bool> IsGrantedAsync(string deviceCode)
    {
        if (_cache.TryGetValue(deviceCode, out var data))
        {
            return Task.FromResult((data.Granted ?? false));
        }

        return Task.FromResult(false);
    }

    public Task CreateAsync(DeviceCodeData data, string clientId, DateTime expiration)
    {
        _cache.TryAdd(data.DeviceCode, new DeviceCodeModel
        {
            ClientId = clientId,
            DeviceCode = data.DeviceCode,
            UserCode = data.UserCode,
            Expiration = expiration,
        });

        return Task.CompletedTask;
    }

    public Task<string> FindSubjectAsync(string deviceCode)
    {
        if (_cache.TryGetValue(deviceCode, out var data))
        {
            return Task.FromResult(data.Subject);
        }

        return Task.FromResult(String.Empty);
    }

    public Task RemoveAsync(string deviceCode)
    {
        _cache.TryRemove(deviceCode, out _);

        return Task.CompletedTask;
    }

    public Task UpdateAsync(string deviceCode, string clientId, ClaimsPrincipal principal, bool granted)
    {
        if (_cache.TryGetValue(deviceCode, out var data))
        {
            data.Subject = principal.GetSubject();
            data.Granted = granted;
        }

        return Task.CompletedTask;
    }

    public Task<bool> IsExistsAsync(string deviceCode, string userCode)
    {
        if (_cache.TryGetValue(deviceCode, out var data))
        {
            return Task.FromResult(data.UserCode == userCode);
        }

        return Task.FromResult(false);
    }

    public Task<bool> IsExistsAsync(string deviceCode)
    {
        if (_cache.TryGetValue(deviceCode, out var data))
        {
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

    public Task<string> FindDeviceCodeAsync(string userCode, string clientId = null)
    {
        var item = _cache.Values.FirstOrDefault(x => x.UserCode == userCode && x.Expiration > DateTime.UtcNow);

        return Task.FromResult(item?.DeviceCode);
    }

    public Task<bool> IsExpirationAsync(string deviceCode)
    {
        if (_cache.TryGetValue(deviceCode, out var data))
        {
            return Task.FromResult(data.Expiration < DateTime.UtcNow);
        }

        return Task.FromResult(true);
    }

    public Task<string> FindClientIdAsync(string deviceCode)
    {
        if (_cache.TryGetValue(deviceCode, out var data))
        {
            return Task.FromResult(data.ClientId);
        }

        return Task.FromResult(string.Empty);
    }

    private class DeviceCodeModel
    {
        public string ClientId { get; set; }
        public string DeviceCode { get; set; }
        public string UserCode { get; set; }
        public DateTime Expiration { get; set; }
        public string Subject { get; set; }
        public bool? Granted { get; set; }
    }
}
