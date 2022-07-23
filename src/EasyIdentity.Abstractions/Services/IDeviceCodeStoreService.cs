using System;
using System.Security.Claims;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public interface IDeviceCodeStoreService
{
    Task CreateAsync(DeviceCodeData data, string clientId, DateTime expiration);

    Task UpdateAsync(string deviceCode, string clientId, ClaimsPrincipal principal, bool granted);

    Task<string> FindDeviceCodeAsync(string userCode, string clientId = null);

    Task<string> FindSubjectAsync(string deviceCode);

    Task<string> FindClientIdAsync(string deviceCode);

    Task<bool> IsGrantedAsync(string deviceCode);

    Task<bool> IsExistsAsync(string deviceCode, string userCode);

    Task<bool> IsExistsAsync(string deviceCode);

    Task<bool> IsExpirationAsync(string deviceCode);

    Task RemoveAsync(string deviceCode);
}
