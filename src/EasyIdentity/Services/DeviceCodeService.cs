using System;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services
{
    public class DeviceCodeService : IDeviceCodeManager
    {
        private readonly IDeviceCodeCodeCreationService _codeCreationService;
        private readonly IDeviceCodeStoreService _deviceCodeStoreService;


        public DeviceCodeService(IDeviceCodeCodeCreationService codeCreationService, IDeviceCodeStoreService deviceCodeStoreService)
        {
            _codeCreationService = codeCreationService;
            _deviceCodeStoreService = deviceCodeStoreService;
        }

        public Task<DeviceCodeAuthenticateResult> AuthenticateAsync(string code)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> FindSubjectAsync(string code)
        {
            throw new System.NotImplementedException();
        }

        public async Task<DeviceCodeData> RequestAsync(RequestData requestData, RequestValidationResult validationResult)
        {
            var client = validationResult.Client;

            var deviceCode = await _codeCreationService.CreateDeviceCodeAsync(client);
            var userCode = await _codeCreationService.CreateUserCodeAsync(client);

            // TODO 
            var data = new DeviceCodeData
            {
                DeviceCode = deviceCode,
                UserCode = userCode,
                ExpiresIn = 180,
                Interval = 5,
                VerificationUri = "",
            };

            await _deviceCodeStoreService.CreateAsync(data, client, DateTime.UtcNow.AddSeconds(180));

            return data;
        }

        public Task<bool> ValidateAsync(string code)
        {
            throw new System.NotImplementedException();
        }
    }
}
