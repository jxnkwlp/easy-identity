using System;
using System.Security.Claims;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services
{
    public interface IDeviceCodeStoreService
    {
        Task CreateAsync(DeviceCodeData data, Client client, DateTime expiration);

        Task UpdateAsync(string deviceCode, ClaimsPrincipal principal);

        Task<string> GetSubjectAsync(string deviceCode);

        Task RemoveAsync(string deviceCode);
    }
}
