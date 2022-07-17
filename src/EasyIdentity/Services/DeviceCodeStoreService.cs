using System;
using System.Security.Claims;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services
{
    public class DeviceCodeStoreService : IDeviceCodeStoreService
    {
        public Task<string> GetSubjectAsync(string deviceCode)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(string deviceCode)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(DeviceCodeData data, Client client, DateTime expiration)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(string deviceCode, ClaimsPrincipal principal)
        {
            throw new NotImplementedException();
        }
    }
}
