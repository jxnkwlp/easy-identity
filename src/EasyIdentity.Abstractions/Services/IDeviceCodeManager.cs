using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services
{
    public interface IDeviceCodeManager
    {
        Task<DeviceCodeData> RequestAsync(RequestData requestData, RequestValidationResult validationResult);

        Task<bool> ValidateAsync(string code);

        Task<string> FindSubjectAsync(string code);

        Task<DeviceCodeAuthenticateResult> AuthenticateAsync(string code);
    }
}
