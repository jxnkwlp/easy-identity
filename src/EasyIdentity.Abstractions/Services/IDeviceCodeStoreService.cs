using System;
using System.Security.Claims;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services
{
    public interface IDeviceCodeStoreService
    {
        Task SaveAsync(string deviceCode, string userCode, Client client, DateTime expiration);

        Task UpdateSubjectAsync(string deviceCode, ClaimsPrincipal principal);

        Task<string> GetSubjectAsync(string deviceCode);

        Task RemoveAsync(string deviceCode);
    }
}
