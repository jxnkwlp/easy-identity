using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services
{
    public class DeviceCodeService : IDeviceCodeService
    {
        private readonly IDeviceCodeCodeCreationService _codeCreationService;
        private readonly IDeviceCodeStoreService _deviceCodeStoreService;


        public DeviceCodeService(IDeviceCodeCodeCreationService codeCreationService, IDeviceCodeStoreService deviceCodeStoreService)
        {
            _codeCreationService = codeCreationService;
            _deviceCodeStoreService = deviceCodeStoreService;
        }

        public async Task<DeviceCodeRequestResult> CodeRequestAsync(RequestData requestData, RequestValidationResult validationResult)
        {
            var client = validationResult.Client;

            var deviceCode = await _codeCreationService.CreateCodeAsync(client);
            var userCode = await _codeCreationService.CreateUserCodeAsync(client);

            await _deviceCodeStoreService.SaveAsync(deviceCode, userCode, client, System.DateTime.UtcNow.AddSeconds(180));

            // TODO 
            return new DeviceCodeRequestResult
            {
                DeviceCode = deviceCode,
                UserCode = userCode,
                ExpiresIn = 180,
                Interval = 5,
                VerificationUri = "",
            };
        }
    }
}
