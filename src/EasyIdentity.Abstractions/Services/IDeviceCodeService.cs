using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services
{
    public interface IDeviceCodeService
    {
        Task<DeviceCodeRequestResult> CodeRequestAsync(RequestData requestData, RequestValidationResult validationResult);
    }
}
