using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services
{
    public class DeviceCodeCodeCreationService : IDeviceCodeCodeCreationService
    {
        public Task<string> CreateCodeAsync(Client client)
        {
            return Task.FromResult(Guid.NewGuid().ToString());
        }

        public Task<string> CreateUserCodeAsync(Client client)
        {
            var result = RandomNumberGenerator.GetInt32(1000, 10000);

            return Task.FromResult(result.ToString());
        }
    }
}
